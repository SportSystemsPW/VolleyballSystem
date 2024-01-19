using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volleyball.DbServices.Services;
using Volleyball.DTO.Matches;

namespace Volleyball.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchDbService matchDbService = new MatchDbService();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var result = await matchDbService.GetMatchByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMatch(NewMatchDto match)
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Unauthorized();
            }
            var result = await matchDbService.AddMatch(match);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateMatch(ManageMatchDto match)
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Unauthorized();
            }
            var result = await matchDbService.UpdateMatch(match);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            string? userId = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }
            var result = await matchDbService.DeleteMatch(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpGet]
        [Route("referees")]
        public async Task<IActionResult> GetReferees()
        {
            string? userId = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }
            return Ok(await matchDbService.GetReferees());
        }

        [HttpGet]
        public async Task<IActionResult> GetMatches([FromQuery]int leagueId, [FromQuery]int seasonId, [FromQuery]int roundId, [FromQuery]int teamId)
        {
            if(leagueId == 0 && seasonId == 0 && roundId == 0 && teamId == 0)
            {
                return Ok(await matchDbService.GetAllMatchesAsync());
            }

            if(seasonId != 0 && teamId != 0)
            {
                return Ok(await matchDbService.GetMatches(seasonId, teamId));
            }

            return Ok(await matchDbService.GetMatches(leagueId, seasonId, roundId));
        }
    }
}
