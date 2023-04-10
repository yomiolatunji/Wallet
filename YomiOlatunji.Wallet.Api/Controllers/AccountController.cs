using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var loggedIn = await _userService.Login(loginRequest);
            var response = loggedIn.status ? ApiResponse<string>.Success(loggedIn.token) : ApiResponse<string>.Failed("");
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var loggedIn = await _userService.ChangePassword(request);
            var response = loggedIn.status ? ApiResponse<string>.Success(loggedIn.message) : ApiResponse<string>.Failed(loggedIn.message);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("admin/login")]
        public async Task<ActionResult> AdminLogin(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var loggedIn = await _userService.AdminLogin(loginRequest);
            var response = loggedIn.status ? ApiResponse<string>.Success(loggedIn.token) : ApiResponse<string>.Failed("");
            return Ok(response);
        }
    }
}