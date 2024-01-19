using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volleyball.DbServices.Services;
using Volleyball.DTO.Teams;

namespace Volleyball.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonController : ControllerBase
    {
        private readonly SeasonDbService seasonDbService = new SeasonDbService();
        [HttpGet]
        public async Task<IActionResult> GetAllSeasons()
        {
            return Ok(await seasonDbService.GetAllSeasonsAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSeason(SeasonDto season)
        {
            await seasonDbService.CreateSeason(season);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeason(SeasonDto updatedSeason)
        {
            await seasonDbService.UpdateSeason(updatedSeason);
            return Ok();
        }
    }
}
