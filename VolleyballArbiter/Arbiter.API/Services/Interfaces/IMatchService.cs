using Arbiter.API.Data.Models;
using Arbiter.API.DTOs;
using Arbiter.API.Enums;

namespace Arbiter.API.Services.Interfaces
{
    public interface IMatchService
    {
        IEnumerable<MatchDto> GetMatches(DateTime fromDate, DateTime toDate);
        MatchDto GetMatch(int matchId);
        Task<MatchInfoDto> GetMatchInfo(int matchId);
        Task<List<MatchReportDto>> GetMatchReport(int matchId);
        Task<Statuses> GetMatchStatus(int matchId);
        Statuses GetSetStatus(int matchId, int setNumber);
        string GetMatchScore(int matchId);
        Task StartMatch(int matchId);
        Task FinishMatch(int matchId);
        Task StartSet(int matchId, int setNumber);
        Task FinishSet(int matchId, int setNumber);
        bool IsMatchInProgress(int matchId);
    }
}
