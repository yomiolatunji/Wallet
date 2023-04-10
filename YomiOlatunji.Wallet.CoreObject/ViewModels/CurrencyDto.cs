namespace YomiOlatunji.Wallet.CoreObject.ViewModels
{
    public class CurrencyDto
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string Symbol { get; set; } = null!;

        public string? CurrencyLogoUrl { get; set; }
        public byte[]? CurrencyLogo { get; set; }
    }
}