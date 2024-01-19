using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Teams;
using Volleyball.Infrastructure.Database.Models;
using VolleyballDomain.Shared;

namespace Volleyball.DbServices.Services
{
    public class SeasonDbService
    {
        private readonly VolleyballContext _context;

        public SeasonDbService()
        {
            _context = new VolleyballContext();
        }

        public async Task<ServiceResponse<List<SeasonDto>>> GetAllSeasonsAsync()
        {
            var response = new ServiceResponse<List<SeasonDto>>();

            response.Data = (await _context.Seasons.OrderByDescending(s => s.Id).ToListAsync()).Select(s => (SeasonDto)s).ToList();

            return response;
        }

        public async Task<ServiceResponse> CreateSeason(SeasonDto season)
        {
            var response = new ServiceResponse();

            try
            {
                await _context.Seasons.AddAsync((Season)season);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Błąd w trakcie zapisu danych";

            }
            return response;
        }

        public async Task<ServiceResponse> UpdateSeason(SeasonDto updatedSeason)
        {
            var response = new ServiceResponse();
            try
            {
                var season = await _context.Seasons.FindAsync(updatedSeason.Id);
                if (season == null)
                {
                    response.Success = false;
                    response.Message = "Nie znaleziono sezonu";
                    return response;
                }
                season.Name = updatedSeason.Name;
                _context.Seasons.Update(season);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Błąd w trakcie zapisu danych";
            }
            return response;
        }
    }
}
