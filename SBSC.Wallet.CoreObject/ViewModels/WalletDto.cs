using System.ComponentModel.DataAnnotations;

namespace SBSC.Wallet.CoreObject.ViewModels
{
    public class WalletDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string WalletNumber { get; set; }
        public string Currency { get; set; }
        public decimal AcyBalance { get; set; }
        public decimal LcyBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal LienAmount { get; set; }
        public decimal InterestReceiveable { get; set; }
        public bool? DebitRestricted { get; set; }
        public bool? CreditRestricted { get; set; }
        public bool? IsActive { get; set; }
        public virtual UserDto User { get; set; } = null!;
    }
    public class AddWalletRequest
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Currency { get; set; }
        public virtual UserDto User { get; set; } = null!;
    }
    public class EditWalletRequest
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Currency { get; set; }
        public virtual UserDto User { get; set; } = null!;
    }
}
