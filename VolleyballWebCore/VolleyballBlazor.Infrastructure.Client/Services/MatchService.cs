using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Matches;
using Volleyball.DTO.Teams;

namespace VolleyballBlazor.Infrastructure.Client.Services
{
    public interface IMatchService
    {
        public Task<ApiResponse<List<VenueDto>>> GetVenues();
        public Task<ApiResponse<List<RoundDto>>> GetRounds();
        public Task<ApiResponse<List<RoundDto>>> GetRounds(int seasonId);
        public Task<ApiResponse<List<LeagueDto>>> GetLeagues();
        public Task<ApiResponse<List<SeasonDto>>> GetSeasons();
        public Task<ApiResponse<MatchDto>> GetMatch(int matchId);
        public Task<ApiResponse<List<MatchSummaryDto>>> GetMatches(int seasonId, int leagueId, int roundId);
        public Task<ApiResponse<List<MatchSummaryDto>>> GetMatches(int seasonId, int teamId);

    }
    public class MatchService : IMatchService
    {
        private readonly HttpClient httpClient;

        public MatchService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiResponse<List<VenueDto>>> GetVenues()
        {
            var response = await httpClient.GetAsync("api/venue");
            return new ApiResponse<List<VenueDto>>(response);
        }

        public async Task<ApiResponse<List<RoundDto>>> GetRounds()
        {
            var response = await httpClient.GetAsync("api/round");
            return new ApiResponse<List<RoundDto>>(response);
        }

        public async Task<ApiResponse<List<LeagueDto>>> GetLeagues()
        {
            var response = await httpClient.GetAsync("api/league");
            return new ApiResponse<List<LeagueDto>>(response);
        }

        public async Task<ApiResponse<MatchDto>> GetMatch(int matchId)
        {
            var response = await httpClient.GetAsync($"api/match/{matchId}");
            return new ApiResponse<MatchDto>(response);
        }
        public async Task<ApiResponse<List<SeasonDto>>> GetSeasons()
        {
            var response = await httpClient.GetAsync("api/season");
            return new ApiResponse<List<SeasonDto>>(response);
        }

        public async Task<ApiResponse<List<RoundDto>>> GetRounds(int seasonId)
        {
            var response = await httpClient.GetAsync($"api/round?seasonId={seasonId}");
            return new ApiResponse<List<RoundDto>>(response);
        }

        public async Task<ApiResponse<List<MatchSummaryDto>>> GetMatches(int seasonId, int leagueId, int roundId)
        {
            var response = await httpClient.GetAsync($"api/match?seasonId={seasonId}&leagueId={leagueId}&roundId={roundId}");
            return new ApiResponse<List<MatchSummaryDto>>(response);
        }

        public async Task<ApiResponse<List<MatchSummaryDto>>> GetMatches(int seasonId, int teamId)
        {
            var response = await httpClient.GetAsync($"api/match?seasonId={seasonId}&teamId={teamId}");
            return new ApiResponse<List<MatchSummaryDto>>(response);
        }

    }
}
