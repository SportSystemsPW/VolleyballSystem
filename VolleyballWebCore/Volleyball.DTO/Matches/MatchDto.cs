using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Teams;
using Volleyball.Infrastructure.Database.Models;

namespace Volleyball.DTO.Matches
{

    public partial class MatchDto
    {
        public int Id { get; set; }

        public DateTime Schedule { get; set; }

        public DateTime CreationDate { get; set; }

        public string? VenueName { get; set; }

        public VenueDto? Venue { get; set; }

        public LeagueDto League { get; set; } = null!;

        public int Sector { get; set; }

        public int Team1Score { get; set; }

        public int Team2Score { get; set; }

        public string? RoundName { get; set; }

        public PlayerSummaryDto? Referee { get; set; }

        public string? UnknownRefereeName { get; set; }

        public string? MatchInfo { get; set; }

        public PlayerSummaryDto? Mvp { get; set; }

        public int? Set1Team1Score { get; set; }

        public int? Set2Team1Score { get; set; }

        public int? Set3Team1Score { get; set; }

        public int? Set4Team1Score { get; set; }

        public int? Set5Team1Score { get; set; }

        public int? Set1Team2Score { get; set; }

        public int? Set2Team2Score { get; set; }

        public int? Set3Team2Score { get; set; }

        public int? Set4Team2Score { get; set; }

        public int? Set5Team2Score { get; set; }

        public string? MatchLeague { get; set; }

        public TeamDto HomeTeam { get; set; } = null!;

        public TeamDto GuestTeam { get; set; } = null!;

        public static explicit operator MatchDto(Match match)
        {
            return new MatchDto
            {
                Id = match.Id,
                Schedule = match.Schedule,
                CreationDate = match.CreationDate,
                VenueName = match.Venue?.Name,
                League = (LeagueDto)match.League,
                Sector = match.Sector,
                Team1Score = match.Team1Score,
                Team2Score = match.Team2Score,
                RoundName = match.Round?.Name,
                Referee = (PlayerSummaryDto?)match.Referee,
                UnknownRefereeName = match.UnknownRefereeName,
                MatchInfo = match.MatchInfo,
                Mvp = (PlayerSummaryDto?)match.Mvp,
                Set1Team1Score = match.Set1Team1Score,
                Set2Team1Score = match.Set2Team1Score,
                Set3Team1Score = match.Set3Team1Score,
                Set4Team1Score = match.Set4Team1Score,
                Set5Team1Score = match.Set5Team1Score,
                Set1Team2Score = match.Set1Team2Score,
                Set2Team2Score = match.Set2Team2Score,
                Set3Team2Score = match.Set3Team2Score,
                Set4Team2Score = match.Set4Team2Score,
                Set5Team2Score = match.Set5Team2Score,
                MatchLeague = match.MatchLeague,
                HomeTeam = (TeamDto)match.HomeTeam,
                GuestTeam = (TeamDto)match.GuestTeam,
                Venue = match.Venue == null ? null : (VenueDto)match.Venue
            };
        }
    }
}
