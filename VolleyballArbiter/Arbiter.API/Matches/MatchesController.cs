using Arbiter.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Arbiter.API.Controllers
{
    //Temporary mock
    [ApiController]
    [Route("matches")]
    public class MatchesController : ControllerBase
    {
        [HttpGet("{matchId:int}")]
        [ProducesResponseType(typeof(MatchDto), 200)]
        public async Task<IActionResult> GetMatch(int matchId)
        {
            return Ok(new MatchDto
            {
                TeamA = new TeamDto
                {
                    Id = 1,
                    Name = "Skra Be³chatów"
                },
                TeamB = new TeamDto
                {
                    Id = 2,
                    Name = "Jastrzêbski Wêgiel"
                },
                Finished = false,
                Score = new MatchScoreDto
                {
                    TeamASetScore = 0,
                    TeamBSetScore = 0,
                    Sets = []
                }
            });
        }

        [HttpPut("{matchId:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutMatch([FromRoute] int matchId, [FromBody] MatchDto matchDto)
        {
            return NoContent();
        }
    }
}
