using System.ComponentModel.DataAnnotations;

namespace YomiOlatunji.Wallet.CoreObject.ViewModels
{
    public class AddWalletRequest
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public string Currency { get; set; }
    }
}