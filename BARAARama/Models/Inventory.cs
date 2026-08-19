using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BARAARama.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public int MaterialId { get; set; }

        public Material Material { get; set; } = null!;

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}