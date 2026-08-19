using System.ComponentModel.DataAnnotations;

namespace BARAARama.ViewModels
{
    public class CreateSupplyViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Supplier Number")]
        public string SupplierNumber { get; set; } = "";

        [Required]
        [StringLength(150)]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; } = "";

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Product Number")]
        public string ProductNumber { get; set; } = "";

        [Required]
        [StringLength(150)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = "";

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Range(
            typeof(decimal),
            "0.01",
            "999999999",
            ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}