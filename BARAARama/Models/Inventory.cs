using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BARAARama.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [ForeignKey(nameof(MaterialId))]
        public Material Material { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }
    }
}