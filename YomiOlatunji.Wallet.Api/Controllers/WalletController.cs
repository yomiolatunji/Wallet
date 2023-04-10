using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;
using YomiOlatunji.Wallet.CoreObject.Enumerables;
using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.Responses;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YomiOlatunji.Wallet.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;

        public WalletController(IWalletService walletService, ITransactionService transactionService)
        {
            _walletService = walletService;
            _transactionService = transactionService;
        }

        [HttpGet(Name = "GetWallets")]
        [ProducesResponseType(typeof(PagedApiResponse<WalletDto>), StatusCodes.Status200OK)]
        public ActionResult<PagedApiResponse<WalletDto>> Get([FromQuery] PagedRequest request)
        {
            var wallets = _walletService.GetWallets(request);
            PagedApiResponse<WalletDto> response;
            if (wallets != null)
            {
                response = PagedApiResponse<WalletDto>.Success(wallets);
                var previousPageLink = wallets.HasPrevious ?
                        CreateWalletResourceUri(request,
                        ResourceUriType.PreviousPage,
                        "GetWallets") : null;

                var nextPageLink = wallets.HasNext ?
                    CreateWalletResourceUri(request,
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

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WalletDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WalletDto>>>> GetWalletByUser(long userId)
        {
            var wallet = await _walletService.GetWalletByUser(userId);
            var response = wallet != null ? ApiResponse<IEnumerable<WalletDto>>.Success(wallet) : ApiResponse<IEnumerable<WalletDto>>.Failed(null);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddWallet([FromBody] AddWalletRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var saved = await _walletService.CreateWallet(request);
            var response = saved.status ? ApiResponse<bool>.Success(true) : ApiResponse<bool>.Failed(false);
            return Ok(response);
        }

        [HttpPost("credit-wallet")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult> CreditWallet(FundWalletRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }

            var saved = await _transactionService.FundWallet(request);
            var response = saved.status ? ApiResponse<string>.Success(saved.message) : ApiResponse<string>.Failed(saved.message);
            return Ok(response);
        }

        [HttpPost("debit-wallet")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult> DebitWallet(FundWalletRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }

            var saved = await _transactionService.DebitWallet(request);
            var response = saved.status ? ApiResponse<string>.Success(saved.message) : ApiResponse<string>.Failed(saved.message);
            return Ok(response);
        }

        [HttpGet("user-transactions", Name = "GetUserTransactions")]
        [ProducesResponseType(typeof(PagedApiResponse<TransactionDto>), StatusCodes.Status200OK)]
        public ActionResult<PagedApiResponse<TransactionDto>> GetTransaction([FromQuery] UserTransactionRequest request)
        {
            var transations = _transactionService.GetTransactionsByUser(request);
            PagedApiResponse<TransactionDto> response;
            if (transations != null)
            {
                response = PagedApiResponse<TransactionDto>.Success(transations);
                var previousPageLink = transations.HasPrevious ?
                        CreateWalletResourceUri(request,
                        ResourceUriType.PreviousPage,
                        "GetUserTransactions") : null;

                var nextPageLink = transations.HasNext ?
                    CreateWalletResourceUri(request,
                    ResourceUriType.NextPage,
                    "GetUserTransactions") : null;

                var paginationMetadata = new
                {
                    totalCount = transations.TotalCount,
                    pageSize = transations.PageSize,
                    currentPage = transations.CurrentPage,
                    totalPages = transations.TotalPages,
                    previousPageLink,
                    nextPageLink
                };

                Response.Headers.Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));
            }
            else
            {
                response = PagedApiResponse<TransactionDto>.NoRecordFound(null);
            }
            return response;
        }

        private string? CreateWalletResourceUri(
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