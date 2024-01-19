using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volleyball.DbServices.Services;
using Volleyball.DTO.Matches;

namespace Volleyball.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly RoundDbService roundDbService = new RoundDbService();
        [HttpGet]
        public async Task<IActionResult> GetAllRounds([FromQuery]int? seasonId)
        {
            return Ok(await roundDbService.GetAllRoundsAsync(seasonId)) ;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRound(RoundDto round)
        {
            await roundDbService.CreateRound(round);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRound(int id, RoundDto updatedRound)
        {
            await roundDbService.UpdateRound(updatedRound);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRound(int id)
        {
            await roundDbService.DeleteRound(id);
            return Ok();
        }
    }
}
