using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YomiOlatunji.Wallet.CoreObject.Enumerables;

namespace YomiOlatunji.Wallet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AdminController : ControllerBase
    {
    }
}