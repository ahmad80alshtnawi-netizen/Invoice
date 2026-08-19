using BARAARama.Models;
using Microsoft.EntityFrameworkCore;

namespace BARAARama.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Inventory> Inventories { get; set; } = null!;


        public DbSet<Cashier> Cashiers { get; set; }

        public DbSet<CashierWithdrawal> CashierWithdrawals { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<SupplyEntry> SupplyEntries { get; set; }
        
    }
}