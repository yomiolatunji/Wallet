using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.CoreObject.Requests
{
    public class TransactionRequest : PagedRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}