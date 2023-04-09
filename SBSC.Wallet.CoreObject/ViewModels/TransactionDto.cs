namespace SBSC.Wallet.CoreObject.ViewModels
{
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
