using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Matches;
using Volleyball.DTO.Teams;
using Volleyball.Infrastructure.Database.Models;
using VolleyballDomain.Shared;

namespace Volleyball.DbServices.Services
{
    public class MatchDbService
    {
        private readonly VolleyballContext _context;
        public MatchDbService()
        {
            _context = new VolleyballContext();
        }

        public async Task<ServiceResponse<List<MatchSummaryDto>>> GetAllMatchesAsync()
        {
            var response = new ServiceResponse<List<MatchSummaryDto>>();

            var matches = (await _context.Matches
                .Include(m => m.League)
                .Include(m => m.HomeTeam)
                .Include(m => m.Mvp)
                .ToListAsync()).Select(m => (MatchSummaryDto)m);

            response.Data = matches.ToList();

            return response;

        }

        public async Task<ServiceResponse<MatchDto>> GetMatchByIdAsync(int id)
        {
            var response = new ServiceResponse<MatchDto>();
            var match = await _context.Matches
                .Include(m => m.League)
                .Include(m => m.GuestTeam).ThenInclude(t => t.TeamPlayers).ThenInclude(tp => tp.Player)
                .Include(m => m.HomeTeam).ThenInclude(t => t.TeamPlayers).ThenInclude(tp => tp.Player)
                .Include(m => m.Mvp)
                .Include(m => m.Round)
                .Include(m => m.Venue)
                .Include(m => m.Referee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono meczu";
                return response;
            }

            response.Data = (MatchDto)match;
            return response;
        }

        public async Task<ServiceResponse<List<MatchSummaryDto>>> GetMatchesByLeagueIdAsync(int id)
        {
            var response = new ServiceResponse<List<MatchSummaryDto>>();
            var matches = (await _context.Matches
                               .Include(m => m.League)
                               .Include(m => m.HomeTeam)
                               .Include(m => m.Mvp)
                               .Where(m => m.LeagueId == id)
                               .ToListAsync()).Select(m => (MatchSummaryDto)m);
            response.Data = matches.ToList();
            return response;
        }

        public async Task<ServiceResponse<List<MatchSummaryDto>>> GetMatchesByLeagueAndTeam(int leagueId, int teamId)
        {
            var response = new ServiceResponse<List<MatchSummaryDto>>();
            var matches = (await _context.Matches
                .Include(m => m.League)
                .Include(m => m.HomeTeam)
                .Include(m => m.Mvp)
                .Where(m => m.LeagueId == leagueId && (m.HomeTeamId == teamId || m.GuestTeamId == teamId))
                .ToListAsync()).Select(m => (MatchSummaryDto)m);
            response.Data = matches.ToList();
            return response;
        }

        public async Task<ServiceResponse<List<MatchSummaryDto>>> GetScheduledMatchesAsync(int seasonId, int roundId, int leagueId)
        {
            var response = new ServiceResponse<List<MatchSummaryDto>>();
            var matches = (await _context.Matches
                .Include(m => m.League)
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Referee).ThenInclude(r => r.Position)
                .Include(m => m.Mvp)
                .Include(m => m.Round)
                .Where(m => m.LeagueId == leagueId && m.RoundId == roundId && m.Round.SeasonId == seasonId)
                .ToListAsync()).Select(m => (MatchSummaryDto)m);

            response.Data = matches.ToList();
            return response;
        }

        public async Task<ServiceResponse> AddMatch(NewMatchDto match)
        {
            var response = new ServiceResponse();
            var matchEntity = (Match)match;
            await _context.Matches.AddAsync(matchEntity);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse> UpdateMatch(ManageMatchDto match)
        {
            var response = new ServiceResponse();

            var matchToUpdate = await _context.Matches.FirstOrDefaultAsync(m => m.Id == match.Id);

            if (matchToUpdate == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono meczu";
                return response;
            }

            matchToUpdate.Team1Score = (byte)match.Team1Score;
            matchToUpdate.Team2Score = (byte)match.Team2Score;
            matchToUpdate.MvpId = match.MvpId;
            matchToUpdate.RefereeId = match.RefereeId;
            matchToUpdate.Sector = (byte)match.Sector;
            matchToUpdate.Schedule = match.Schedule;
            matchToUpdate.VenueId = match.VenueId ?? matchToUpdate.VenueId;
            matchToUpdate.UnknownRefereeName = match.UnknownRefereeName;
            matchToUpdate.MatchInfo = match.MatchInfo;
            matchToUpdate.Set1Team1Score = (byte?)match.Set1Team1Score;
            matchToUpdate.Set1Team2Score = (byte?)match.Set1Team2Score;
            matchToUpdate.Set2Team1Score = (byte?)match.Set2Team1Score;
            matchToUpdate.Set2Team2Score = (byte?)match.Set2Team2Score;
            matchToUpdate.Set3Team1Score = (byte?)match.Set3Team1Score;
            matchToUpdate.Set3Team2Score = (byte?)match.Set3Team2Score;
            matchToUpdate.Set4Team1Score = (byte?)match.Set4Team1Score;
            matchToUpdate.Set4Team2Score = (byte?)match.Set4Team2Score;
            matchToUpdate.Set5Team1Score = (byte?)match.Set5Team1Score;
            matchToUpdate.Set5Team2Score = (byte?)match.Set5Team2Score;
            try
            {
                _context.Update(matchToUpdate);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }


        }

        public async Task<ServiceResponse> DeleteMatch(int id)
        {
            var response = new ServiceResponse();
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono meczu";
                return response;
            }
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<List<PlayerSummaryDto>>> GetReferees()
        {
            var response = new ServiceResponse<List<PlayerSummaryDto>>();

            List<User> referees = await _context.Users
                .Include(p => p.Team)
                .Include(p => p.Credentials)
                .Where(p => p.Credentials != null)
                .Include(p => p.Credentials)
                .ThenInclude(c => c!.Roles)
                .Where(p => p.Credentials!.Roles.Any(r => r.Name == Roles.Arbiter))
                .ToListAsync();

            response.Data = referees.Select(r => (PlayerSummaryDto)r!).ToList();

            return response;
        }

        public async Task<ServiceResponse<List<MatchSummaryDto>>> GetMatches(int leagueId, int seasonId, int roundId)
        {
            var response = new ServiceResponse<List<MatchSummaryDto>>();
            var matches = await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Mvp)
                .Include(m => m.Referee)
                .Include(m => m.Venue)
                .Include(m => m.Round)
                .Where(m => m.Round.SeasonId == seasonId && m.LeagueId == leagueId && m.RoundId == roundId)
                .ToListAsync();

            response.Data = matches.Select(m => (MatchSummaryDto)m).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<MatchSummaryDto>>> GetMatches(int seasonId, int teamId)
        {
            var response = new ServiceResponse<List<MatchSummaryDto>>();
            var matches = await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.GuestTeam)
                .Include(m => m.Mvp)
                .Include(m => m.Referee)
                .Include(m => m.Venue)
                .Include(m => m.Round)
                .Where(m => m.Round.SeasonId == seasonId && (m.HomeTeamId == teamId || m.GuestTeamId == teamId))
                .ToListAsync();

            response.Data = matches.Select(m => (MatchSummaryDto)m).ToList();
            return response;
        }
    }
}
