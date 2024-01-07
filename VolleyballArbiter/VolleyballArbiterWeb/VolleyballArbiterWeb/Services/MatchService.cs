using VolleyballArbiterWeb.DTOs;
using VolleyballArbiterWeb.Models;
using VolleyballArbiterWeb.Services.Interfaces;

namespace VolleyballArbiterWeb.Services
{
    public class MatchService : IMatchService
    {
        private HttpClient _httpClient;

        public MatchService()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                return true;
            };
            _httpClient = new HttpClient(handler);
        }

        public async Task<List<MatchModel>> GetMatches(DateTime fromDate, DateTime toDate)
        {
            var response = await _httpClient.GetAsync(AppStrings.Api.BaseUrl + $"/matches?fromDate={fromDate.ToString("yyyy-MM-dd")}&toDate={toDate.ToString("yyyy-MM-dd")}");
            var matchDtoList = await response.Content.ReadFromJsonAsync<IEnumerable<MatchDto>>();
            var matchList = new List<MatchModel>();

            if (matchDtoList is null) return matchList;

            foreach (var matchDto in matchDtoList)
            {
                var homeLogoUrl = "";
                var guestLogoUrl = "";

                if (matchDto.HomeTeamLogo is null)
                {
                    homeLogoUrl = "Images/volleyball.png";
                }
                else
                {
                    var source = Convert.ToBase64String(matchDto.HomeTeamLogo!);
                    homeLogoUrl = string.Format("data:image/jpeg;base64,{0}", source);
                }

                if (matchDto.GuestTeamLogo is null)
                {
                    guestLogoUrl = "Images/volleyball.png";
                }
                else
                {
                    var source = Convert.ToBase64String(matchDto.GuestTeamLogo!);
                    guestLogoUrl = string.Format("data:image/jpeg;base64,{0}", source);
                }

                matchList.Add(new MatchModel()
                {
                    Id = matchDto.Id,
                    HomeTeam = matchDto.HomeTeam,
                    HomeTeamLogoURL = homeLogoUrl,
                    GuestTeam = matchDto.GuestTeam,
                    GuestTeamLogoURL = guestLogoUrl,
                    LeagueName = matchDto.LeagueName,
                    MatchDate = matchDto.MatchDateTime.ToString("d"),
                    MatchTime = matchDto.MatchDateTime.ToString("H:mm")
                });
            }

            return matchList;
        }

        public async Task<MatchModel> GetMatch(int matchId)
        {
            var response = await _httpClient.GetAsync(AppStrings.Api.BaseUrl + $"/match?matchId={matchId}");
            var matchDto = await response.Content.ReadFromJsonAsync<MatchDto>();

            var homeLogoUrl = "";
            var guestLogoUrl = "";

            if (matchDto.HomeTeamLogo is null)
            {
                homeLogoUrl = "Images/volleyball.png";
            }
            else
            {
                var source = Convert.ToBase64String(matchDto.HomeTeamLogo!);
                homeLogoUrl = string.Format("data:image/jpeg;base64,{0}", source);
            }

            if (matchDto.GuestTeamLogo is null)
            {
                guestLogoUrl = "Images/volleyball.png";
            }
            else
            {
                var source = Convert.ToBase64String(matchDto.GuestTeamLogo!);
                guestLogoUrl = string.Format("data:image/jpeg;base64,{0}", source);
            }

            var match = new MatchModel()
            {
                Id = matchDto.Id,
                HomeTeam = matchDto.HomeTeam,
                HomeTeamLogoURL = homeLogoUrl,
                GuestTeam = matchDto.GuestTeam,
                GuestTeamLogoURL = guestLogoUrl,
                LeagueName = matchDto.LeagueName,
                MatchDate = matchDto.MatchDateTime.ToString("d"),
                MatchTime = matchDto.MatchDateTime.ToString("H:mm")
            };

            return match;
        }

        public async Task<MatchInfoModel> GetMatchInfo(int matchId)
        {
            var response = await _httpClient.GetAsync(AppStrings.Api.BaseUrl + $"/match/matchInfo?matchId={matchId}");
            var matchInfo = await response.Content.ReadFromJsonAsync<MatchInfoModel>();

            return matchInfo ?? new MatchInfoModel();
        }

        public async Task<List<MatchReportModel>> GetMatchReport(int matchId)
        {
            var response = await _httpClient.GetAsync(AppStrings.Api.BaseUrl + $"/match/matchReport?matchId={matchId}");
            var matchReport = await response.Content.ReadFromJsonAsync<List<MatchReportModel>>();

            return matchReport ?? new List<MatchReportModel>();
        }

        public async Task<string> GetMatchScore(int matchId)
        {
            var response = await _httpClient.GetAsync(AppStrings.Api.BaseUrl + $"/match/score?matchId={matchId}");
            var matchScore = await response.Content.ReadAsStringAsync();

            return matchScore ?? "0:0";
        }
    }
}
