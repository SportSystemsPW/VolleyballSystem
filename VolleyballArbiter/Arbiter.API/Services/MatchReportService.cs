using Arbiter.API.Data;
using Arbiter.API.Data.Models;
using Arbiter.API.Enums;
using Arbiter.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using static Arbiter.API.AppStrings.AppStrings;

namespace Arbiter.API.Services
{
    public class MatchReportService : IMatchReportService
    {
        private readonly DatabaseContext _dbContext;

        public MatchReportService()
        {
            _dbContext = new DatabaseContext();
        }

        public async Task SaveAction(int matchId, string action, string arbiterSentence)
        {
            if (!IsMatchInProgress(matchId)) throw new ArgumentException("Match is not in progress");

            var setInProgress = GetSetInProgress(matchId);

            _dbContext.MatchReports.Add(new MatchReport 
            { 
                MatchId = matchId,
                Action = action,
                ArbiterSentence = arbiterSentence,
                Set = setInProgress > 0 ? setInProgress : throw new Exception("There is no set in progress"),
                Timestamp = DateTime.Now
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeScore(int matchId, string action)
        {
            if (!IsMatchInProgress(matchId)) throw new ArgumentException("Match is not in progress");

            var setInProgress = GetSetInProgress(matchId);
            if (setInProgress < 0) throw new Exception("There is no set in progress");

            string setScore = "0:0";

            switch (setInProgress)
            {
                case 1:
                    setScore = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).Select(x => x.Set1Score).FirstOrDefault()!;
                    break;
                case 2:
                    setScore = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).Select(x => x.Set2Score).FirstOrDefault()!;
                    break;
                case 3:
                    setScore = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).Select(x => x.Set3Score).FirstOrDefault()!;
                    break;
                case 4:
                    setScore = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).Select(x => x.Set4Score).FirstOrDefault()!;
                    break;
                case 5:
                    setScore = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).Select(x => x.Set5Score).FirstOrDefault()!;
                    break;
                default:
                    throw new ArgumentException("Given set number is wrong");
            }

            var teamScores = setScore.Split(":");
            var homeTeamScore = Int32.Parse(teamScores[0]);
            var guestTeamScore = Int32.Parse(teamScores[1]);

            if (action == Actions.HOME_TEAM_POINT) homeTeamScore++;
            if (action == Actions.GUEST_TEAM_POINT) guestTeamScore++;

            var newScore = homeTeamScore + ":" + guestTeamScore;
            var matchStatus = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault()!;
            switch (setInProgress)
            {
                case 1:
                    matchStatus.Set1Score = newScore;
                    break;
                case 2:
                    matchStatus.Set2Score = newScore;
                    break;
                case 3:
                    matchStatus.Set3Score = newScore;
                    break;
                case 4:
                    matchStatus.Set4Score = newScore;
                    break;
                case 5:
                    matchStatus.Set5Score = newScore;
                    break;
                default:
                    throw new ArgumentException("Given set number is wrong");
            }

            await _dbContext.SaveChangesAsync();
        }

        private int GetSetInProgress(int matchId)
        {
            var matchStatus = _dbContext.MatchStatuses.AsNoTracking().Where(x => x.MatchId == matchId).FirstOrDefault();

            if (IsSetInProgress(matchStatus!, 1)) return 1;
            if (IsSetInProgress(matchStatus!, 2)) return 2;
            if (IsSetInProgress(matchStatus!, 3)) return 3;
            if (IsSetInProgress(matchStatus!, 4)) return 4;
            if (IsSetInProgress(matchStatus!, 5)) return 5;

            return -1;
        }

        private bool IsSetInProgress(MatchStatus matchStatus, int setNumber)
        {
            switch (setNumber)
            {
                case 1:
                    if (matchStatus.Set1Status == (int)Statuses.IN_PROGRESS) return true;
                    return false;
                case 2:
                    if (matchStatus.Set2Status == (int)Statuses.IN_PROGRESS) return true;
                    return false;
                case 3:
                    if (matchStatus.Set3Status == (int)Statuses.IN_PROGRESS) return true;
                    return false;
                case 4:
                    if (matchStatus.Set4Status == (int)Statuses.IN_PROGRESS) return true;
                    return false;
                case 5:
                    if (matchStatus.Set5Status == (int)Statuses.IN_PROGRESS) return true;
                    return false;
                default:
                    throw new ArgumentException("Given set number is wrong");
            }
        }

        public bool IsMatchInProgress(int matchId) => _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault() is not null &&
            _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault()!.MatchStatus1 == (int)Statuses.IN_PROGRESS;
    }
}
