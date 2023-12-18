using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int GetTrainerId()
        {
            return 4;
        }

        protected TrainingOrganizerResponse<T> CreateResponse<T>(T content, List<string>? errors = null)
        {
            return new TrainingOrganizerResponse<T>
            {
                Content = content,
                Messages = errors
            };
        }
    }
}
