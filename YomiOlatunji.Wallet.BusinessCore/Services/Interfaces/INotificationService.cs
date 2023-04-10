using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Services.Interfaces
{
    public interface INotificationService
    {
        PagedList<NotificationDto> GetNotifications(UserPagedRequest request);
        NotificationDto GetNotification(long id);
    }
}
