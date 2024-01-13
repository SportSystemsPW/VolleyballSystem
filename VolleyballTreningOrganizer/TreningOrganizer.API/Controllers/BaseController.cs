using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int GetTrainerId()
        {
            return int.Parse(User.Claims.First(c => c.Type == "UserId").Value ?? "-1");
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
