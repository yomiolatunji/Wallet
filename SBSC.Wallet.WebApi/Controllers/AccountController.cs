using Microsoft.AspNetCore.Mvc;
using SBSC.Wallet.BusinessCore.Services.Interfaces;
using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var loggedIn = await _userService.Login(loginRequest);
            var response = loggedIn.status ? APIResponse<string>.Success(loggedIn.token) : APIResponse<string>.Failed("");
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
            var response = loggedIn.status ? APIResponse<string>.Success(loggedIn.message) : APIResponse<string>.Failed(loggedIn.message);
            return Ok(response);
        }
        [HttpPost("admin/login")]
        public async Task<ActionResult> AdminLogin(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var loggedIn = await _userService.AdminLogin(loginRequest);
            var response = loggedIn.status ? APIResponse<string>.Success(loggedIn.token) : APIResponse<string>.Failed("");
            return Ok(response);
        }
    }
}
