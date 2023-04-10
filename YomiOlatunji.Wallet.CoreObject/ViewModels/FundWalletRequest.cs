using System.ComponentModel.DataAnnotations;

namespace YomiOlatunji.Wallet.CoreObject.ViewModels
{
    public class FundWalletRequest
    {
        [Required]
        public long WalletId { get; set; }

        [Required]
        public string Currency { get; set; }

        public decimal Amount { get; set; }
        public long UserId { get; set; }
        public string? Narration { get; set; }
    }
}