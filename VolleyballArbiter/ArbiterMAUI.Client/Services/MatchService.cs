using ArbiterMAUI.Client.DTOs;
using ArbiterMAUI.Client.Models;
using ArbiterMAUI.Client.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ArbiterMAUI.Client.Services
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
            _httpClient.DefaultRequestHeaders.Add("ApiKey", App._refereeKey);
        }

        public async Task<IEnumerable<Match>> DownloadMatches(DateTime fromDate, DateTime toDate)
        {
            var response = await _httpClient.GetAsync(AppStrings.Api.BaseUrl + $"/matches?fromDate={fromDate.ToString("yyyy-MM-dd")}&toDate={toDate.ToString("yyyy-MM-dd")}");
            var matchDtoList = await response.Content.ReadFromJsonAsync<IEnumerable<MatchDto>>();
            var matchList = new List<Match>();

            foreach (var matchDto in matchDtoList)
            {
                ImageSource homeLogo;
                ImageSource guestLogo;
                
                if(matchDto.HomeTeamLogo is null)
                {
                    homeLogo = ImageSource.FromFile("volleyball.png");
                }
                else
                {
                    homeLogo = ImageSource.FromStream(() => new MemoryStream(matchDto.HomeTeamLogo));
                }

                if (matchDto.GuestTeamLogo is null)
                {
                    guestLogo = ImageSource.FromFile("volleyball.png");
                }
                else
                {
                    guestLogo = ImageSource.FromStream(() => new MemoryStream(matchDto.GuestTeamLogo));
                }

                matchList.Add(new Match
                {
                    Id = matchDto.Id,
                    HomeTeam = matchDto.HomeTeam,
                    HomeTeamLogo = homeLogo,
                    GuestTeam = matchDto.GuestTeam,
                    GuestTeamLogo = guestLogo,
                    LeagueName= matchDto.LeagueName,
                    MatchDate = matchDto.MatchDateTime.ToString("d"),
                    MatchTime = matchDto.MatchDateTime.ToString("H:mm")
                });
            }
            return matchList;
        }

        public async Task<int> GetMatchStatus(int matchId)
        {
            var response = await _httpClient.GetAsync(AppStrings.Api.BaseUrl + $"/match/status?matchId={matchId}");
            var text = await response.Content.ReadAsStringAsync();

            return Int32.Parse(text);
        }

        public async Task<int> GetSetStatus(int matchId, int setNumber)
        {
            var response = await _httpClient.GetAsync(AppStrings.Api.BaseUrl + $"/match/set/status?matchId={matchId}&setNumber={setNumber}");
            var text = await response.Content.ReadAsStringAsync();

            return Int32.Parse(text);
        }

        public async Task<bool> StartMatch(int matchId)
        {
            var response = await _httpClient.PostAsync(AppStrings.Api.BaseUrl + $"/match/start?matchId={matchId}", null);

            if (response.IsSuccessStatusCode) return true;
            else return false;
        }

        public async Task<bool> FinishMatch(int matchId)
        {
            var response = await _httpClient.PostAsync(AppStrings.Api.BaseUrl + $"/match/finish?matchId={matchId}", null);

            if (response.IsSuccessStatusCode) return true;
            else return false;
        }

        public async Task<bool> StartSet(int matchId, int setNumber)
        {
            var response = await _httpClient.PostAsync(AppStrings.Api.BaseUrl + $"/match/set/start?matchId={matchId}&setNumber={setNumber}", null);

            if (response.IsSuccessStatusCode) return true;
            else return false;
        }

        public async Task<bool> FinishSet(int matchId, int setNumber)
        {
            var response = await _httpClient.PostAsync(AppStrings.Api.BaseUrl + $"/match/set/finish?matchId={matchId}&setNumber={setNumber}", null);

            if (response.IsSuccessStatusCode) return true;
            else return false;
        }
    }
}
