using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SBSC.Wallet.BusinessCore.Services.Interfaces;
using SBSC.Wallet.CoreObject.Enumerables;
using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(typeof(PagedApiResponse<UserDto>), StatusCodes.Status200OK)]
        public ActionResult<PagedApiResponse<UserDto>> Get(PagedRequest request)
        {
            var users = _userService.GetUsers(request);
            PagedApiResponse<UserDto> response;
            if (users != null)
            {
                response = PagedApiResponse<UserDto>.Success(users);
                var previousPageLink = users.HasPrevious ?
                        CreateUserResourceUri(request,
                        ResourceUriType.PreviousPage,
                        "GetUsers") : null;

                var nextPageLink = users.HasNext ?
                    CreateUserResourceUri(request,
                    ResourceUriType.NextPage,
                    "GetUsers") : null;

                var paginationMetadata = new
                {
                    totalCount = users.TotalCount,
                    pageSize = users.PageSize,
                    currentPage = users.CurrentPage,
                    totalPages = users.TotalPages,
                    previousPageLink,
                    nextPageLink
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));
            }
            else
            {
                response = PagedApiResponse<UserDto>.NoRecordFound(null);
            }
            return response;
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddUser([FromBody] AddUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var saved = await _userService.Add(request);
            var response = saved.status ? APIResponse<bool>.Success(true) : APIResponse<bool>.Failed(false);
            return Ok(response);
        }
        [HttpPut]
        [ProducesResponseType(typeof(APIResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> EditUser([FromBody] EditUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var saved = await _userService.Edit(request);
            var response = saved.status ? APIResponse<bool>.Success(true) : new APIResponse<bool>() { Message = saved.message, Data = false, Code = ResponseCodes.Failed.code };
            return Ok(response);
        }

        private string? CreateUserResourceUri(
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
