using System.ComponentModel.DataAnnotations;

namespace InventoryInvoiceApp.Models
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }

        [Required]
        [StringLength(100)]
        public string MaterialName { get; set; } = "";
    }
}