namespace YomiOlatunji.Wallet.CoreObject.ViewModels
{
    public class JwtTokenClass
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}