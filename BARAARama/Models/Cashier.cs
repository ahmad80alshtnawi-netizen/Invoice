using System.ComponentModel.DataAnnotations;

namespace BARAARama.Models
{
    public class Cashier
    {
        [Key]
        public int CashierId { get; set; }

        [Required]
        [StringLength(50)]
        public string CashierNumber { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string CashierName { get; set; } = "";

        public DateTime DateTime { get; set; } = DateTime.Now;

        public ICollection<CashierWithdrawal> Withdrawals { get; set; }
            = new List<CashierWithdrawal>();
    }
}