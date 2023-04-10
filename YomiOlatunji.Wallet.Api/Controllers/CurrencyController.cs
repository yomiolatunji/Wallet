using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.BusinessCore.Services;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;
using YomiOlatunji.Wallet.CoreObject.Enumerables;
using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.Responses;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        [HttpGet(Name = "GetCurrencies")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CurrencyDto>>), StatusCodes.Status200OK)]
        public ActionResult<ApiResponse<IEnumerable<CurrencyDto>>> Get()
        {
            var currencies = _currencyService.GetCurrencies();
            var response = currencies != null ? ApiResponse<IEnumerable<CurrencyDto>>.Success(currencies) 
                : ApiResponse<IEnumerable<CurrencyDto>>.Failed(currencies);
            return Ok(response);
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddCurrency([FromBody] AddCurrencyRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var saved = await _currencyService.Add(request);
            var response = saved.status ? ApiResponse<bool>.Success(true) : ApiResponse<bool>.Failed(false);
            return Ok(response);
        }
    }
}