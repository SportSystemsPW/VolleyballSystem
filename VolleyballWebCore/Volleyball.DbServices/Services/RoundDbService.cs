using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Matches;
using Volleyball.Infrastructure.Database.Models;
using VolleyballDomain.Shared;

namespace Volleyball.DbServices.Services
{
    public class RoundDbService
    {
        private readonly VolleyballContext _context;
        public RoundDbService()
        {
            _context = new VolleyballContext();
        }

        public async Task<ServiceResponse<List<RoundDto>>> GetAllRoundsAsync(int? seasonId)
        {
            var response = new ServiceResponse<List<RoundDto>>();
            if (seasonId != null)
            {
                response.Data = (await _context.Rounds.OrderDescending().Where(r => r.Season.Id == seasonId).ToListAsync()).Select(r => (RoundDto)r).ToList();
                return response;
            }
            response.Data = (await _context.Rounds.ToListAsync()).Select(r => (RoundDto)r).ToList();
            return response;
        }

        public async Task<ServiceResponse> CreateRound(RoundDto round)
        {
            var response = new ServiceResponse();
            try
            {
                await _context.Rounds.AddAsync((Round)round);
                await _context.SaveChangesAsync();
            }
            catch
            {
                response.Success = false;
                response.Message = "Błąd w trakcie zapisu danych";
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateRound(RoundDto updatedRound)
        {
            var response = new ServiceResponse();
            try
            {
                var round = await _context.Rounds.FindAsync(updatedRound.Id);
                if (round == null)
                {
                    response.Success = false;
                    response.Message = "Nie znaleziono rundy";
                    return response;
                }
                round.Name = updatedRound.Name;
                round.SeasonId = updatedRound.SeasonId;
                await _context.SaveChangesAsync();
            }
            catch
            {
                response.Success = false;
                response.Message = "Błąd w trakcie zapisu danych";
            }
            return response;
        }

        public async Task<ServiceResponse> DeleteRound(int id)
        {
            var response = new ServiceResponse();
            try
            {
                var round = await _context.Rounds.FindAsync(id);
                if (round == null)
                {
                    response.Success = false;
                    response.Message = "Nie znaleziono rundy";
                    return response;
                }
                _context.Rounds.Remove(round);
                await _context.SaveChangesAsync();
            }
            catch
            {
                response.Success = false;
                response.Message = "Błąd w trakcie zapisu danych";
            }
            return response;
        }
    }
}
