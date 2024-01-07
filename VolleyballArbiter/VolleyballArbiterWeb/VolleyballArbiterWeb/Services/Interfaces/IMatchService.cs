using VolleyballArbiterWeb.Models;

namespace VolleyballArbiterWeb.Services.Interfaces
{
    public interface IMatchService
    {
        Task<List<MatchModel>> GetMatches(DateTime fromDate, DateTime toDate);
        Task<MatchModel> GetMatch(int matchId);
        Task<MatchInfoModel> GetMatchInfo(int matchId);
        Task<List<MatchReportModel>> GetMatchReport(int matchId);
        Task<string> GetMatchScore(int matchId);
    }
}
