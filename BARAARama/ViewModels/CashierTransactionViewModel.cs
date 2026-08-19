using System.ComponentModel.DataAnnotations;

namespace BARAARama.ViewModels
{
    public class CashierTransactionViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Cashier Number")]
        public string CashierNumber { get; set; } = "";

        [Required]
        [StringLength(100)]
        [Display(Name = "Cashier Name")]
        public string CashierName { get; set; } = "";

        public List<CashierWithdrawalViewModel> Withdrawals { get; set; }
            = new List<CashierWithdrawalViewModel>();
    }

    public class CashierWithdrawalViewModel
    {
        [Required]
        [Display(Name = "Material")]
        public int MaterialId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
    }
}