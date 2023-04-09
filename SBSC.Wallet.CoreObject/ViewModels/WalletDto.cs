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
    public class TransactionDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string TransactionType { get; set; } = null!;

        public string Currency { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? Narration { get; set; }

        public string? TransactionCode { get; set; }
    }
}
