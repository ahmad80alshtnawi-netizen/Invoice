using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BARAARama.Models
{
    public class SupplyEntry
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Product Number")]
        public string ProductNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Range(0.1, double.MaxValue)]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }
    }
}
