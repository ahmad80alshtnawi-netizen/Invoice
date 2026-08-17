using System.ComponentModel.DataAnnotations;

namespace BARAARama.Models
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
