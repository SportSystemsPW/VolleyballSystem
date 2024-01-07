using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arbiter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefereeCheckController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult CheckApiKey()
        {
            return Ok();
        }
    }
}
