using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YomiOlatunji.Wallet.BusinessCore.Services;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;
using YomiOlatunji.Wallet.CoreObject.Enumerables;
using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;

        public AdminController(IUserService userService, ITransactionService transactionService)
        {
            _userService = userService;
            _transactionService = transactionService;
        }

        [Authorize(Roles = UserRoles.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddAdmin([FromBody] AddAdminRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var saved = await _userService.AddAdmin(request);
            var response = saved.status ? ApiResponse<bool>.Success(true) : ApiResponse<bool>.Failed(false);
            return Ok(response);
        }

        [Authorize(Roles = UserRoles.SuperAdmin)]
        [HttpPost("superadmin")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddSuperAdmin([FromBody] AddAdminRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var saved = await _userService.AddSuperAdmin(request);
            var response = saved.status ? ApiResponse<bool>.Success(true) : ApiResponse<bool>.Failed(false);
            return Ok(response);
        }

        [HttpPost("user/activate")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> ActivateUser([FromBody] long userId)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var saved = await _userService.ActivateUser(userId);
            var response = saved.status ? ApiResponse<bool>.Success(true) : ApiResponse<bool>.Failed(false);
            return Ok(response);
        }

        [HttpPost("user/deactivate")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeactivateUser([FromBody] long userId)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var saved = await _userService.DeactivateUser(userId);
            var response = saved.status ? ApiResponse<bool>.Success(true) : ApiResponse<bool>.Failed(false);
            return Ok(response);
        }
        [HttpGet("transactions/download")]
        public ActionResult DownloadTransactions([FromQuery] DownloadRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var res = _transactionService.DownloadTransactions(request.StartDate, request.EndDate);

            var fileName = $@"Transactions_{DateTime.Today.ToString("d")}.xlsx";
            return File(res, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}