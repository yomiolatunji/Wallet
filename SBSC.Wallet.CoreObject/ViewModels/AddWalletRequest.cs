using System.ComponentModel.DataAnnotations;

namespace SBSC.Wallet.CoreObject.ViewModels
{
    public class AddWalletRequest
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public string Currency { get; set; }
    }
}