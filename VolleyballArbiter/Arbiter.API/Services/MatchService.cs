using Arbiter.API.Data;
using Arbiter.API.Data.Models;
using Arbiter.API.DTOs;
using Arbiter.API.Enums;
using Arbiter.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Arbiter.API.Services
{
    public class MatchService : IMatchService
    {
        private readonly DatabaseContext _dbContext;

        public MatchService(DatabaseContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public IEnumerable<MatchDto> GetMatches(DateTime fromDate, DateTime toDate)
        {
            var matches = _dbContext.Matches.Where(x => x.Schedule > fromDate && x.Schedule < toDate)
                .Include(x => x.HomeTeam)
                .Include(x => x.GuestTeam)
                .Include(x => x.League);

            var matchDtoList = new List<MatchDto>();
            foreach (var match in matches)
            {
                matchDtoList.Add(new MatchDto
                {
                    Id = match.Id,
                    HomeTeam = match.HomeTeam.Name,
                    HomeTeamLogo = match.HomeTeam.Logo,
                    GuestTeam = match.GuestTeam.Name,
                    GuestTeamLogo = match.GuestTeam.Logo,
                    LeagueName = match.League.Name,
                    MatchDateTime = match.Schedule
                });
            }
            return matchDtoList;
        }

        public MatchDto GetMatch(int matchId)
        {
            var match = _dbContext.Matches.Where(x => x.Id == matchId)
                .Include(x => x.HomeTeam)
                .Include(x => x.GuestTeam)
                .Include(x => x.League)
                .FirstOrDefault();

            var matchDto = new MatchDto()
            {
                Id = match.Id,
                HomeTeam = match.HomeTeam.Name,
                HomeTeamLogo = match.HomeTeam.Logo,
                GuestTeam = match.GuestTeam.Name,
                GuestTeamLogo = match.GuestTeam.Logo,
                LeagueName = match.League.Name,
                MatchDateTime = match.Schedule
            };

            return matchDto;
        }

        public async Task<List<MatchReportDto>> GetMatchReport(int matchId)
        {
            var matchReport = await _dbContext.MatchReports.Where(x => x.MatchId == matchId).ToListAsync();
            var matchReportDtoList = new List<MatchReportDto>();

            foreach(var report in matchReport)
            {
                matchReportDtoList.Add(new MatchReportDto()
                {
                    MatchId = report.MatchId,
                    Action = report.Action,
                    ArbiterSentence = report.ArbiterSentence,
                    Set = report.Set,
                    Timestamp = report.Timestamp
                });
            }

            return matchReportDtoList;
        }

        public async Task<MatchInfoDto> GetMatchInfo(int matchId)
        {
            var matchStatus = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault();

            if (matchStatus is null)
            {
                matchStatus = new MatchStatus()
                {
                    MatchId = matchId,
                    MatchStatus1 = (int)Statuses.UPCOMING,
                    Set1Status = (int)Statuses.UPCOMING,
                    Set2Status = (int)Statuses.UPCOMING,
                    Set3Status = (int)Statuses.UPCOMING,
                    Set4Status = (int)Statuses.UPCOMING,
                    Set5Status = (int)Statuses.UPCOMING,
                };

                _dbContext.MatchStatuses.Add(matchStatus);
                await _dbContext.SaveChangesAsync();
            }

            var matchInfo = new MatchInfoDto()
            {
                MatchId = matchStatus.MatchId,
                MatchScore = matchStatus.MatchScore,
                MatchStatus = matchStatus.MatchStatus1,
                Set1Status = matchStatus.Set1Status,
                Set2Status = matchStatus.Set2Status,
                Set3Status = matchStatus.Set3Status,
                Set4Status = matchStatus.Set4Status,
                Set5Status = matchStatus.Set5Status,
                Set1Score = matchStatus.Set1Score,
                Set2Score = matchStatus.Set2Score,
                Set3Score = matchStatus.Set3Score,
                Set4Score = matchStatus.Set4Score,
                Set5Score = matchStatus.Set5Score
            };

            return matchInfo;
        }

        public async Task<Statuses> GetMatchStatus(int matchId)
        {
            var matchStatus = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault();

            if (matchStatus is null)
            {
                _dbContext.MatchStatuses.Add(new MatchStatus()
                {
                    MatchId = matchId,
                    MatchStatus1 = (int)Statuses.UPCOMING,
                    Set1Status = (int)Statuses.UPCOMING,
                    Set2Status = (int)Statuses.UPCOMING,
                    Set3Status = (int)Statuses.UPCOMING,
                    Set4Status = (int)Statuses.UPCOMING,
                    Set5Status = (int)Statuses.UPCOMING,
                });
                await _dbContext.SaveChangesAsync();

                return Statuses.UPCOMING;
            }     
            else return (Statuses)matchStatus.MatchStatus1!;
        }

        public string GetMatchScore(int matchId)
        {
            var matchScore = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault();

            return matchScore.MatchScore ?? "0:0";
        }

        public Statuses GetSetStatus(int matchId, int setNumber)
        {
            var matchStatus = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault();

            if(setNumber == 1)
            {
                return (Statuses)matchStatus!.Set1Status!;
            }
            if (setNumber == 2)
            {
                return (Statuses)matchStatus!.Set2Status!;
            }
            if (setNumber == 3)
            {
                return (Statuses)matchStatus!.Set3Status!;
            }
            if (setNumber == 4)
            {
                return (Statuses)matchStatus!.Set4Status!;
            }
            if (setNumber == 5)
            {
                return (Statuses)matchStatus!.Set5Status!;
            }

            throw new Exception("Wrong set number");
        }

        public async Task StartMatch(int matchId)
        {
            var match = _dbContext.Matches.Where(x => x.Id == matchId).FirstOrDefault();
            if(match is null)
            {
                throw new ArgumentException("There is no match with given id");
            }

            var matchStatus = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault();
            if (matchStatus is null)
            {
                throw new ArgumentException("There is no match status with given matchid");
            }

            if (matchStatus is not null && (matchStatus.MatchStatus1 != (int)Statuses.UPCOMING))
            {
                throw new ArgumentException("Cannot start given match, because it was already started in past");
            }
            else if (matchStatus is not null || (matchStatus.MatchStatus1 == (int)Statuses.UPCOMING || matchStatus.MatchStatus1 is null))
            {
                matchStatus.MatchStatus1 = (int)Statuses.IN_PROGRESS;
                await _dbContext.SaveChangesAsync();
                return;
            }

            _dbContext.MatchStatuses.Add(new MatchStatus
            {
                MatchId = matchId
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task FinishMatch(int matchId)
        {
            var match = _dbContext.Matches.Where(x => x.Id == matchId).FirstOrDefault();
            if (match is null)
            {
                throw new ArgumentException("There is no match with given id");
            }

            var matchStatus = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault();
            if (matchStatus is not null && IsMatchInProgress(matchId))
            {
                matchStatus.MatchStatus1 = (int)Statuses.FINISHED;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Cannot finish match because it is not in progress");
            }
        }

        public async Task StartSet(int matchId, int setNumber)
        {
            var match = _dbContext.Matches.Where(x => x.Id == matchId).FirstOrDefault();
            if (match is null) throw new ArgumentException("There is no match with given id");

            var matchStatus = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault();
            if (matchStatus is not null && IsMatchInProgress(matchId))
            {
                switch (setNumber)
                {
                    case 1:
                        matchStatus.Set1Status = (int)Statuses.IN_PROGRESS;
                        matchStatus.Set1Score = "0:0";
                        break;
                    case 2:
                        matchStatus.Set2Status = (int)Statuses.IN_PROGRESS;
                        matchStatus.Set2Score = "0:0";
                        break;
                    case 3:
                        matchStatus.Set3Status = (int)Statuses.IN_PROGRESS;
                        matchStatus.Set3Score = "0:0";
                        break;
                    case 4:
                        matchStatus.Set4Status = (int)Statuses.IN_PROGRESS;
                        matchStatus.Set4Score = "0:0";
                        break;
                    case 5:
                        matchStatus.Set5Status = (int)Statuses.IN_PROGRESS;
                        matchStatus.Set5Score = "0:0";
                        break;
                    default:
                        throw new ArgumentException("Given set number is wrong");
                }
                await _dbContext.SaveChangesAsync();
            }
            else throw new Exception("Cannot start given set in given match");
        }

        public async Task FinishSet(int matchId, int setNumber)
        {
            var match = _dbContext.Matches.Where(x => x.Id == matchId).FirstOrDefault();
            if (match is null) throw new ArgumentException("There is no match with given id");

            var matchStatus = _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault();
            if (matchStatus is not null && IsMatchInProgress(matchId))
            {
                switch (setNumber)
                {
                    case 1:
                        matchStatus.Set1Status = (int)Statuses.FINISHED;

                        var teamSetScores = matchStatus.Set1Score!.Split(":");
                        var homeTeamSetScore = Int32.Parse(teamSetScores[0]);
                        var guestTeamSetScore = Int32.Parse(teamSetScores[1]);

                        matchStatus.MatchScore =  matchStatus.MatchScore is null ? "0:0" : matchStatus.MatchScore;
                        var teamMatchScores = matchStatus.MatchScore!.Split(":");
                        var homeTeamMatchScore = Int32.Parse(teamMatchScores[0]);
                        var guestTeamMatchScore = Int32.Parse(teamMatchScores[1]);
                        if (homeTeamSetScore > guestTeamSetScore) 
                        {
                            homeTeamMatchScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        else if (homeTeamSetScore < guestTeamSetScore)
                        {
                            guestTeamMatchScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        break;
                    case 2:
                        matchStatus.Set2Status = (int)Statuses.FINISHED;

                        teamSetScores = matchStatus.Set2Score!.Split(":");
                        homeTeamSetScore = Int32.Parse(teamSetScores[0]);
                        guestTeamSetScore = Int32.Parse(teamSetScores[1]);

                        matchStatus.MatchScore = matchStatus.MatchScore is null ? "0:0" : matchStatus.MatchScore;
                        teamMatchScores = matchStatus.MatchScore!.Split(":");
                        homeTeamMatchScore = Int32.Parse(teamMatchScores[0]);
                        guestTeamMatchScore = Int32.Parse(teamMatchScores[1]);
                        if (homeTeamSetScore > guestTeamSetScore)
                        {
                            homeTeamMatchScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        else if (homeTeamSetScore < guestTeamSetScore)
                        {
                            guestTeamSetScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        break;
                    case 3:
                        matchStatus.Set3Status = (int)Statuses.FINISHED;

                        teamSetScores = matchStatus.Set3Score!.Split(":");
                        homeTeamSetScore = Int32.Parse(teamSetScores[0]);
                        guestTeamSetScore = Int32.Parse(teamSetScores[1]);

                        matchStatus.MatchScore = matchStatus.MatchScore is null ? "0:0" : matchStatus.MatchScore;
                        teamMatchScores = matchStatus.MatchScore!.Split(":");
                        homeTeamMatchScore = Int32.Parse(teamMatchScores[0]);
                        guestTeamMatchScore = Int32.Parse(teamMatchScores[1]);
                        if (homeTeamSetScore > guestTeamSetScore)
                        {
                            homeTeamMatchScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        else if (homeTeamSetScore < guestTeamSetScore)
                        {
                            guestTeamSetScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        break;
                    case 4:
                        matchStatus.Set4Status = (int)Statuses.FINISHED;

                        teamSetScores = matchStatus.Set4Score!.Split(":");
                        homeTeamSetScore = Int32.Parse(teamSetScores[0]);
                        guestTeamSetScore = Int32.Parse(teamSetScores[1]);

                        matchStatus.MatchScore = matchStatus.MatchScore is null ? "0:0" : matchStatus.MatchScore;
                        teamMatchScores = matchStatus.MatchScore!.Split(":");
                        homeTeamMatchScore = Int32.Parse(teamMatchScores[0]);
                        guestTeamMatchScore = Int32.Parse(teamMatchScores[1]);
                        if (homeTeamSetScore > guestTeamSetScore)
                        {
                            homeTeamMatchScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        else if (homeTeamSetScore < guestTeamSetScore)
                        {
                            guestTeamSetScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        break;
                    case 5:
                        matchStatus.Set5Status = (int)Statuses.FINISHED;

                        teamSetScores = matchStatus.Set5Score!.Split(":");
                        homeTeamSetScore = Int32.Parse(teamSetScores[0]);
                        guestTeamSetScore = Int32.Parse(teamSetScores[1]);

                        matchStatus.MatchScore = matchStatus.MatchScore is null ? "0:0" : matchStatus.MatchScore;
                        teamMatchScores = matchStatus.MatchScore!.Split(":");
                        homeTeamMatchScore = Int32.Parse(teamMatchScores[0]);
                        guestTeamMatchScore = Int32.Parse(teamMatchScores[1]);
                        if (homeTeamSetScore > guestTeamSetScore)
                        {
                            homeTeamMatchScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        else if (homeTeamSetScore < guestTeamSetScore)
                        {
                            guestTeamSetScore++;
                            matchStatus.MatchScore = homeTeamMatchScore.ToString() + ":" + guestTeamMatchScore.ToString();
                        }
                        break;
                    default:
                        throw new ArgumentException("Given set number is wrong");
                }
                await _dbContext.SaveChangesAsync();
            }
            else throw new Exception("Cannot finish given set in given match");
        }

        public bool IsMatchInProgress(int matchId) => _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault() is not null &&
            _dbContext.MatchStatuses.Where(x => x.MatchId == matchId).FirstOrDefault()!.MatchStatus1 == (int)Statuses.IN_PROGRESS;
    }
}
