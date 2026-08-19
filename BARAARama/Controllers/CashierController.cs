using System.Data;
using BARAARama.Data;
using BARAARama.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BARAARama.Controllers
{
    public class CashierController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CashierController> _logger;

        public CashierController(
            AppDbContext context,
            ILogger<CashierController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await LoadCashierViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteSale(
            CashierViewModel model)
        {
            var requestedItems = model.Items
                .Where(x => x.SellQuantity > 0)
                .ToList();

            if (requestedItems.Count == 0)
            {
                ModelState.AddModelError(
                    "",
                    "Select at least one material.");

                return View(
                    "Index",
                    await ReloadWithSelectionsAsync(model));
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                foreach (var requested in requestedItems)
                {
                    var material = await _context.Materials
                        .FirstOrDefaultAsync(x =>
                            x.MaterialId == requested.MaterialId);

                    if (material == null)
                    {
                        ModelState.AddModelError(
                            "",
                            "One selected material no longer exists.");

                        await transaction.RollbackAsync();

                        return View(
                            "Index",
                            await ReloadWithSelectionsAsync(model));
                    }

                    var stockRows = await _context.Inventories
                        .Where(x =>
                            x.MaterialId == requested.MaterialId &&
                            x.Quantity > 0)
                        .OrderBy(x => x.InventoryId)
                        .ToListAsync();

                    var available =
                        stockRows.Sum(x => x.Quantity);

                    if (requested.SellQuantity > available)
                    {
                        ModelState.AddModelError(
                            "",
                            $"Insufficient stock for " +
                            $"{material.MaterialName}. " +
                            $"Available: {available}.");

                        await transaction.RollbackAsync();

                        return View(
                            "Index",
                            await ReloadWithSelectionsAsync(model));
                    }

                    var remaining = requested.SellQuantity;

                    foreach (var stockRow in stockRows)
                    {
                        if (remaining == 0)
                        {
                            break;
                        }

                        var deducted = Math.Min(
                            stockRow.Quantity,
                            remaining);

                        stockRow.Quantity -= deducted;
                        remaining -= deducted;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation(
                    "Cashier completed a sale with {Count} materials.",
                    requestedItems.Count);

                TempData["Success"] =
                    "Sale completed and stock decreased successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();

                _logger.LogError(
                    exception,
                    "Error while completing cashier sale.");

                ModelState.AddModelError(
                    "",
                    "The sale could not be completed.");

                return View(
                    "Index",
                    await ReloadWithSelectionsAsync(model));
            }
        }

        private async Task<CashierViewModel>
            LoadCashierViewModelAsync()
        {
            var stockRows = await _context.Inventories
                .AsNoTracking()
                .Include(x => x.Material)
                .Where(x => x.Quantity > 0)
                .ToListAsync();

            var items = stockRows
                .GroupBy(x => new
                {
                    x.MaterialId,
                    x.Material.MaterialName
                })
                .Select(group => new CashierItemViewModel
                {
                    MaterialId = group.Key.MaterialId,
                    MaterialName = group.Key.MaterialName,
                    AvailableQuantity =
                        group.Sum(x => x.Quantity),

                    Price = group
                        .OrderByDescending(x => x.InventoryId)
                        .First()
                        .Price
                })
                .OrderBy(x => x.MaterialName)
                .ToList();

            return new CashierViewModel
            {
                Items = items
            };
        }

        private async Task<CashierViewModel>
            ReloadWithSelectionsAsync(CashierViewModel submitted)
        {
            var actual = await LoadCashierViewModelAsync();

            foreach (var item in actual.Items)
            {
                var submittedItem =
                    submitted.Items.FirstOrDefault(x =>
                        x.MaterialId == item.MaterialId);

                if (submittedItem != null)
                {
                    item.SellQuantity =
                        submittedItem.SellQuantity;
                }
            }

            return actual;
        }
    }
}