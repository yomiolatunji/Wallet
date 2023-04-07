namespace SBSC.Wallet.CoreObject.Enumerables
{
    public class ResponseCodes
    {
        public static readonly (string code, string message) Success = ("00", "Success");
        public static readonly (string code, string message) BadRequest = ("400", "Bad Request");
        public static readonly (string code, string message) Unauthorized = ("401", "Unauthorized");
        public static readonly (string code, string message) NotFound = ("404", "Not Found");
        public static readonly (string code, string message) Failed = ("99", "Failed");
    }
}
