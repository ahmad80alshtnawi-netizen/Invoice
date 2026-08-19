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

        [ForeignKey(nameof(MaterialId))]
        public Material Material { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string MaterialName { get; set; } = "";

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(
            typeof(decimal),
            "0.01",
            "1000000")]
        public decimal Price { get; set; }

        [Required]
        public int CashierId { get; set; }

        [ForeignKey(nameof(CashierId))]
        public Cashier Cashier { get; set; } = null!;
    }
}