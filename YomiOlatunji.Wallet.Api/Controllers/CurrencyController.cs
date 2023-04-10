using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YomiOlatunji.Wallet.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        public CurrencyController()
        {
        }
    }
}