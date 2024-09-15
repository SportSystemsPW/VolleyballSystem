using Arbiter.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Arbiter.API.Controllers
{
    //Temporary mock
    [ApiController]
    [Route("matches")]
    public class MatchesController : ControllerBase
    {
        private IMemoryCache _memoryCache;
     
        public MatchesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            
            if (!_memoryCache.TryGetValue(1, out var _))
            {
                _memoryCache.Set(1, CreateMock());
            }
        }

        [HttpGet("{matchId:int}")]
        [ProducesResponseType(typeof(MatchDto), 200)]
        public async Task<IActionResult> GetMatch(int matchId)
        {
            if(_memoryCache.TryGetValue(matchId, out var matchDto))
            {
                return Ok(matchDto);
            }
        
            return NotFound();
        }

        [HttpPut("{matchId:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutMatch([FromRoute] int matchId, [FromBody] MatchDto matchDto)
        {
            _memoryCache.Set(matchId, matchDto);

            return NoContent();
        }

        private MatchDto CreateMock()
        {
            return new MatchDto
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
            };
        }
    }
}
 