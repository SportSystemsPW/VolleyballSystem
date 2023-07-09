using Arbiter.API.Services.Interfaces;
using Arbiter.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Arbiter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService= matchService;
        }

        [HttpGet]
        [Route("~/api/matches")]
        public IEnumerable<MatchDto> GetMatchList()
        {
            var matchList = _matchService.GetMatches();

            var matchDtoList = new List<MatchDto>();

            foreach (var match in matchList)
            {
                matchDtoList.Add(new MatchDto
                {
                    Id = match.Id,
                    HomeTeam = match.HomeTeam.Name,
                    HomeTeamLogo = match.HomeTeam.Logo,
                    GuestTeam = match.GuestTeam.Name,
                    GuestTeamLogo = match.GuestTeam.Logo,
                    LeagueName = match.League.Name,
                    MatchDateTime = match.Schedule
                });
            }

            return matchDtoList;
        }
    }
}
