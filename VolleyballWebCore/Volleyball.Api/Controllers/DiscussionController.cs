using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volleyball.DbServices.Services;
using Volleyball.DTO.Discussion;
using VolleyballDomain.Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Volleyball.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private readonly DiscussionDbService discussionDbService = new DiscussionDbService();

        [HttpGet]
        [Route("profilecomments/{id}")]
        public async Task<IActionResult> GetProfileComments(int id)
        {
            var result = await discussionDbService.GetProfileComments(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("teamcomments/{id}")]
        public async Task<IActionResult> GetTeamComments(int id)
        {
            var result = await discussionDbService.GetTeamComments(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("articlecomments/{id}")]
        public async Task<IActionResult> GetArticleComments(int id)
        {
            var result = await discussionDbService.GetArticleComments(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("matchcomments/{id}")]
        public async Task<IActionResult> GetMatchComments(int id)
        {
            var result = await discussionDbService.GetMatchComments(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("articlecomment")]
        public async Task<IActionResult> AddArticleComment(NewCommentDto comment)
        {
            string? user = User.Identity?.Name;

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await discussionDbService.AddArticleComment(comment, user);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("profilecomment")]
        public async Task<IActionResult> AddProfileComment(NewCommentDto comment)
        {
            string? user = User.Identity?.Name;
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await discussionDbService.AddProfileComment(comment, user);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("teamcomment")]
        public async Task<IActionResult> AddTeamComment(NewCommentDto comment)
        {
            string? user = User.Identity?.Name;
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await discussionDbService.AddTeamComment(comment, user);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("matchcomment")]
        public async Task<IActionResult> AddMatchComment(NewCommentDto comment)
        {
            string? user = User.Identity?.Name;
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await discussionDbService.AddMatchComment(comment, user);
            return Ok(result);
        }


        [HttpGet]
        [Route("logs")]
        public async Task<IActionResult> GetLogs([FromQuery] int amount = 10)
        {
            var result = await discussionDbService.GetRecentLogs(amount);
            return Ok(result);
        }

    }
}
