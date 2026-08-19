using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BARAARama.Models
{
    public class CashierWithdrawal
    {
        [Key]
        public int CashierWithdrawalId { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [Required]
        [StringLength(100)]
        public string MaterialName { get; set; } = "";

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int CashierId { get; set; }

        public Cashier Cashier { get; set; } = null!;
    }
}