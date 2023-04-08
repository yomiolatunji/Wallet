using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SBSC.Wallet.BusinessCore.Services;
using SBSC.Wallet.BusinessCore.Services.Interfaces;
using SBSC.Wallet.CoreObject.Enumerables;
using SBSC.Wallet.CoreObject.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SBSC.Wallet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        [HttpGet(Name = "GetWallets")]
        [ProducesResponseType(typeof(PagedApiResponse<WalletDto>), StatusCodes.Status200OK)]
        public ActionResult<PagedApiResponse<WalletDto>> Get(PagedRequest request)
        {
            var wallets = _walletService.GetWallets(request);
            PagedApiResponse<WalletDto> response;
            if (wallets != null)
            {
                response = PagedApiResponse<WalletDto>.Success(wallets);
                var previousPageLink = wallets.HasPrevious ?
                        CreateUserResourceUri(request,
                        ResourceUriType.PreviousPage,
                        "GetWallets") : null;

                var nextPageLink = wallets.HasNext ?
                    CreateUserResourceUri(request,
                    ResourceUriType.NextPage,
                    "GetWallets") : null;

                var paginationMetadata = new
                {
                    totalCount = wallets.TotalCount,
                    pageSize = wallets.PageSize,
                    currentPage = wallets.CurrentPage,
                    totalPages = wallets.TotalPages,
                    previousPageLink,
                    nextPageLink
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));
            }
            else
            {
                response = PagedApiResponse<WalletDto>.NoRecordFound(null);
            }
            return response;
        }

        // GET api/<WalletController>/5
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<WalletDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse<IEnumerable<WalletDto>>>> GetByUser(long userId)
        {
            var wallet = await _walletService.GetWalletByUser(userId);
            var response = wallet != null ? APIResponse<IEnumerable<WalletDto>>.Success(wallet) : APIResponse<IEnumerable<WalletDto>>.Failed(null);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddWallet([FromBody] AddWalletRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var saved = await _walletService.CreateWallet(request);
            var response = saved.status ? APIResponse<bool>.Success(true) : APIResponse<bool>.Failed(false);
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
