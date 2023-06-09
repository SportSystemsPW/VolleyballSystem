using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TreningOrganizer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int GetTrainerId()
        {
            return 2;
        }
    }
}
