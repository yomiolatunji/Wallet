namespace YomiOlatunji.Wallet.CoreObject.Requests
{
    public class AddCurrencyRequest
    {
        public string Name { get; set; } = null!;

        public string Symbol { get; set; } = null!;

        public string? CurrencyLogoBase64 { get; set; }
    }
}