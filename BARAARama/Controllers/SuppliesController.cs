using BARAARama.Data;
using BARAARama.Models;
using BARAARama.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BARAARama.Controllers
{
    public class SuppliesController : Controller
    {
        private readonly AppDbContext _context;

        public SuppliesController(
            AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Create()
        {
            var model =
                new CreateSupplyViewModel
                {
                    Date = DateTime.Now
                };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateSupplyViewModel model)
        {
            model.SupplierNumber =
                model.SupplierNumber?.Trim() ?? "";

            model.SupplierName =
                model.SupplierName?.Trim() ?? "";

            model.ProductNumber =
                model.ProductNumber?.Trim() ?? "";

            model.ProductName =
                model.ProductName?.Trim() ?? "";

            // The server controls the actual transaction time.
            model.Date = DateTime.Now;


            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // Check whether this supplier number
            // was previously used.
            var existingSupplier =
                await _context.Suppliers
                    .AsNoTracking()
                    .Where(s =>
                        s.SupplierNumber ==
                        model.SupplierNumber)
                    .OrderByDescending(s =>
                        s.Date)
                    .FirstOrDefaultAsync();


            // Existing supplier number keeps
            // its previously stored name.
            if (existingSupplier != null)
            {
                model.SupplierName =
                    existingSupplier.SupplierName;
            }


            // Check whether this product number
            // was previously used.
            var existingProduct =
                await _context.SupplyEntries
                    .AsNoTracking()
                    .Where(e =>
                        e.ProductNumber ==
                        model.ProductNumber)
                    .OrderByDescending(e => e.Id)
                    .FirstOrDefaultAsync();


            // Prevent one product number from
            // being associated with different names.
            if (existingProduct != null &&
                !string.Equals(
                    existingProduct.ProductName,
                    model.ProductName,
                    StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(
                    nameof(model.ProductName),
                    $"Product number {model.ProductNumber} already belongs to '{existingProduct.ProductName}'.");

                return View(model);
            }


            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync();


            try
            {
                // -------------------------------------------------
                // 1. Create a new Supplier transaction every time.
                // -------------------------------------------------

                var supplier =
                    new Supplier
                    {
                        SupplierNumber =
                            model.SupplierNumber,

                        SupplierName =
                            model.SupplierName,

                        Date =
                            model.Date
                    };

                _context.Suppliers.Add(supplier);

                await _context.SaveChangesAsync();


                // -------------------------------------------------
                // 2. Find or create the Material master record.
                // -------------------------------------------------

                var material =
                    await _context.Materials
                        .FirstOrDefaultAsync(m =>
                            m.MaterialName ==
                            model.ProductName);


                if (material == null)
                {
                    material =
                        new Material
                        {
                            MaterialName =
                                model.ProductName
                        };

                    _context.Materials.Add(material);

                    // We need MaterialId before creating inventory.
                    await _context.SaveChangesAsync();
                }


                // -------------------------------------------------
                // 3. Create SupplyEntry history.
                // -------------------------------------------------

                var supplyEntry =
                    new SupplyEntry
                    {
                        ProductNumber =
                            model.ProductNumber,

                        ProductName =
                            model.ProductName,

                        Quantity =
                            model.Quantity,

                        Price =
                            model.Price,

                        SupplierId =
                            supplier.Id
                    };

                _context.SupplyEntries.Add(
                    supplyEntry);


                // -------------------------------------------------
                // 4. Create or update current Inventory.
                // -------------------------------------------------

                var inventory =
                    await _context.Inventories
                        .FirstOrDefaultAsync(i =>
                            i.MaterialId ==
                            material.MaterialId);


                if (inventory == null)
                {
                    inventory =
                        new Inventory
                        {
                            MaterialId =
                                material.MaterialId,

                            Quantity =
                                model.Quantity,

                            Price =
                                model.Price
                        };

                    _context.Inventories.Add(
                        inventory);
                }
                else
                {
                    inventory.Quantity +=
                        model.Quantity;

                    // Latest supplier price becomes
                    // the current inventory price.
                    inventory.Price =
                        model.Price;
                }


                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                TempData["SuccessMessage"] =
                    "Supply transaction recorded successfully.";

                return RedirectToAction(
                    nameof(Index));
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var suppliers =
                await _context.Suppliers
                    .AsNoTracking()
                    .Include(s =>
                        s.SupplyEntries)
                    .OrderByDescending(s =>
                        s.Date)
                    .ToListAsync();

            return View(suppliers);
        }


        // Used by JavaScript to auto-fill product name.
        [HttpGet]
        public async Task<IActionResult> GetProduct(
            string productNumber)
        {
            if (string.IsNullOrWhiteSpace(
                productNumber))
            {
                return Json(new
                {
                    found = false
                });
            }


            var product =
                await _context.SupplyEntries
                    .AsNoTracking()
                    .Where(e =>
                        e.ProductNumber ==
                        productNumber.Trim())
                    .OrderByDescending(e => e.Id)
                    .FirstOrDefaultAsync();


            if (product == null)
            {
                return Json(new
                {
                    found = false
                });
            }


            return Json(new
            {
                found = true,
                name = product.ProductName
            });
        }


        // Used by JavaScript to auto-fill supplier name.
        [HttpGet]
        public async Task<IActionResult> GetSupplier(
            string supplierNumber)
        {
            if (string.IsNullOrWhiteSpace(
                supplierNumber))
            {
                return Json(new
                {
                    found = false
                });
            }


            var supplier =
                await _context.Suppliers
                    .AsNoTracking()
                    .Where(s =>
                        s.SupplierNumber ==
                        supplierNumber.Trim())
                    .OrderByDescending(s =>
                        s.Date)
                    .FirstOrDefaultAsync();


            if (supplier == null)
            {
                return Json(new
                {
                    found = false
                });
            }


            return Json(new
            {
                found = true,
                name = supplier.SupplierName
            });
        }
    }
}