using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using YomiOlatunji.Wallet.BusinessCore.Services;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;
using YomiOlatunji.Wallet.CoreObject.Enumerables;
using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.Responses;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(INotificationService notificationService,IMapper mapper)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }
        [HttpGet(Name = "GetNotifications")]
        [ProducesResponseType(typeof(PagedApiResponse<NotificationDto>), StatusCodes.Status200OK)]
        public ActionResult<PagedApiResponse<NotificationDto>> Get([FromQuery] PagedRequest request)
        {
            var userIdStr = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr))
            {
                return BadRequest(PagedApiResponse<NotificationDto>.Failed(null));
            }
            var userId = Convert.ToInt64(userIdStr);
            UserPagedRequest userRequest = _mapper.Map<UserPagedRequest>(request);
            userRequest.UserId = userId;
            var notifications = _notificationService.GetNotifications(userRequest);
            PagedApiResponse<NotificationDto> response;
            if (notifications != null)
            {
                response = PagedApiResponse<NotificationDto>.Success(notifications);
                var previousPageLink = notifications.HasPrevious ?
                        CreateNotificationResourceUri(request,
                        ResourceUriType.PreviousPage,
                        "GetNotifications") : null;

                var nextPageLink = notifications.HasNext ?
                    CreateNotificationResourceUri(request,
                    ResourceUriType.NextPage,
                    "GetNotifications") : null;

                var paginationMetadata = new
                {
                    totalCount = notifications.TotalCount,
                    pageSize = notifications.PageSize,
                    currentPage = notifications.CurrentPage,
                    totalPages = notifications.TotalPages,
                    previousPageLink,
                    nextPageLink
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));
            }
            else
            {
                response = PagedApiResponse<NotificationDto>.NoRecordFound(null);
            }
            return Ok(response);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<NotificationDto>), StatusCodes.Status200OK)]
        public ActionResult<ApiResponse<NotificationDto>> Get(long id)
        {
            var notification = _notificationService.GetNotification(id);
            var response = notification != null ? ApiResponse<NotificationDto>.Success(notification)
                : ApiResponse<NotificationDto>.Failed(notification);
            return Ok(response);
        }
        private string? CreateNotificationResourceUri(
            PagedRequest request,
            ResourceUriType type,
            string actionName)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(actionName,
                        new
                        {
                            pageNumber = request.PageNumber - 1,
                            pageSize = request.PageSize,
                            searchQuery = request.SearchQuery
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(actionName,
                        new
                        {
                            pageNumber = request.PageNumber + 1,
                            pageSize = request.PageSize,
                            searchQuery = request.SearchQuery
                        });

                default:
                    return Url.Link(actionName,
                        new
                        {
                            pageNumber = request.PageNumber,
                            pageSize = request.PageSize,
                            searchQuery = request.SearchQuery
                        });
            }
        }
    }
}
