using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.CoreObject.Requests
{
    public class UserPagedRequest : PagedRequest
    {
        public long UserId { get; set; }
    }
}