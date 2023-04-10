using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;
using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly WalletContext _context;
        private readonly IMapper _mapper;
        public NotificationService(WalletContext context, IMapper mapper, IConfiguration configuration) : base(configuration)
        {
            _context = context;
            _mapper = mapper;
        }

        public NotificationDto GetNotification(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var user = _context.Notifications.FirstOrDefault(a => a.Id == id);
            return _mapper.Map<NotificationDto>(user);
        }

        public PagedList<NotificationDto> GetNotifications(UserPagedRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var collection = _context.Notifications as IQueryable<Notification>;
            collection = collection.Where(a => a.UserId == request.UserId);
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = request.SearchQuery.Trim();
                collection = collection.Where(a => a.Subject.Contains(searchQuery)
                    || a.Message.Contains(searchQuery));
            }

            var users = PagedList<Notification>.ToPagedList(collection,
                request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.SortDirection);

            return _mapper.Map<PagedList<NotificationDto>>(users);
        }
    }
}
