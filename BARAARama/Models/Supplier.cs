using System.ComponentModel.DataAnnotations;

namespace BARAARama.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Supplier Number")]
        public string SupplierNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; } = string.Empty;

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        public ICollection<SupplyEntry> SupplyEntries { get; set; }
            = new List<SupplyEntry>();
    }
}
