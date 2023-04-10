namespace YomiOlatunji.Wallet.BusinessCore.DbModels;

public partial class Transaction
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string TransactionType { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? Narration { get; set; }

    public string? TransactionCode { get; set; }

    public long WalletId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}