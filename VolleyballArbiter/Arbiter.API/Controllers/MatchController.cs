using Arbiter.API.DTOs;
using Arbiter.API.Hubs;
using Arbiter.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Arbiter.API.AppStrings.AppStrings;

namespace Arbiter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly IHubContext<MatchReportTextAnalyzerHub> _hubContext;

        public MatchController(IMatchService matchService, IHubContext<MatchReportTextAnalyzerHub> hubContext)
        {
            _matchService= matchService;
            _hubContext= hubContext;
        }

        [HttpGet]
        [Route("~/api/matches")]
        public IActionResult GetMatchList(DateTime fromDate, DateTime toDate)
        {
            if(toDate < fromDate)
            {
                return BadRequest("The start date cannot be greater than the end date"); // Status Code 400
            }

            var matchDtoList = _matchService.GetMatches(fromDate, toDate);

            if (matchDtoList.Any())
            {
                return Ok(matchDtoList); // Status Code 200
            }
            else
            {
                return StatusCode(StatusCodes.Status204NoContent, "There are no matches in provided date period");
            }
        }

        [HttpGet]
        public IActionResult GetMatch(int matchId)
        {
            var matchDto = _matchService.GetMatch(matchId);

            return Ok(matchDto);
        }

        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> GetMatchStatus(int matchId) 
        { 
            var matchStatus = await _matchService.GetMatchStatus(matchId);

            return Ok((int)matchStatus);
        }

        [HttpGet]
        [Route("set/status")]
        public IActionResult GetSetStatus(int matchId, int setNumber)
        {
            var setStatus = _matchService.GetSetStatus(matchId, setNumber);

            return Ok((int)setStatus);
        }

        [HttpGet]
        [Route("score")]
        public IActionResult GetMatchScore(int matchId)
        {
            var matchScore = _matchService.GetMatchScore(matchId);

            return Ok(matchScore);
        }

        [HttpPost]
        [Route("start")]
        [Authorize]
        public async Task<IActionResult> StartMatch(int matchId)
        {
            await _matchService.StartMatch(matchId);
            await _hubContext.Clients.All.SendAsync("MatchHasBeenStarted", matchId);

            return Ok();
        }

        [HttpPost]
        [Route("finish")]
        [Authorize]
        public async Task<IActionResult> FinishMatch(int matchId)
        {
            Thread.Sleep(100);
            await _matchService.FinishMatch(matchId);
            await _hubContext.Clients.All.SendAsync("MatchHasBeenFinished", matchId);

            return Ok();
        }

        [HttpPost]
        [Route("set/start")]
        [Authorize]
        public async Task<IActionResult> StartSet(int matchId, int setNumber)
        {
            await _matchService.StartSet(matchId, setNumber);
            await _hubContext.Clients.All.SendAsync("SetHasBeenStarted", matchId, setNumber);

            return Ok();
        }

        [HttpPost]
        [Route("set/finish")]
        [Authorize]
        public async Task<IActionResult> FinishSet(int matchId, int setNumber)
        {
            await Task.Delay(100);
            await _matchService.FinishSet(matchId, setNumber);
            await _hubContext.Clients.All.SendAsync("SetHasBeenFinished", matchId);

            return Ok();
        }

        [HttpGet]
        [Route("matchInfo")]
        public async Task<IActionResult> GetMatchInfo(int matchId)
        {
            var matchInfo = await _matchService.GetMatchInfo(matchId);

            return Ok(matchInfo);
        }

        [HttpGet]
        [Route("matchReport")]
        public async Task<IActionResult> GetMatchReport(int matchId)
        {
            var matchReport = await _matchService.GetMatchReport(matchId);

            return Ok(matchReport);
        }
    }
}
