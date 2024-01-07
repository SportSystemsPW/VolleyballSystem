using Arbiter.API.Data;
using Arbiter.API.Enums;
using Arbiter.API.Hubs;
using Arbiter.API.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using static Arbiter.API.AppStrings.AppStrings;

namespace Arbiter.API.Services
{
    public class KeyPhraseAnalyzerService : ITextAnalyzerService
    {
        private const string _homeTeam = "HOME_TEAM";
        private const string _guestTeam = "GUEST_TEAM";
        private const string _point = "POINT";

        private readonly IMatchReportService _matchReportService;
        private readonly IHubContext<MatchReportTextAnalyzerHub> _hubContext;

        public KeyPhraseAnalyzerService(IMatchReportService matchReportService, IHubContext<MatchReportTextAnalyzerHub> hubContext)
        {
            _matchReportService = matchReportService;
            _hubContext = hubContext;
        }

        public async Task AnalyzeSentence(int matchId, string sentence)
        {
            var words = sentence.Replace(".", string.Empty).ToLower().Split(' ').ToList();
            var keyPhrases = GetKeyPhrases(words);

            if(keyPhrases.Contains(_homeTeam) && keyPhrases.Contains(_point))
            {
                await _matchReportService.SaveAction(matchId, Actions.HOME_TEAM_POINT, sentence);
                await _matchReportService.ChangeScore(matchId, Actions.HOME_TEAM_POINT);
                await _hubContext.Clients.All.SendAsync("NewPoint", matchId, Actions.HOME_TEAM_POINT);
                return;
            }
            if (keyPhrases.Contains(_guestTeam) && keyPhrases.Contains(_point))
            {
                await _matchReportService.SaveAction(matchId, Actions.GUEST_TEAM_POINT, sentence);
                await _matchReportService.ChangeScore(matchId, Actions.GUEST_TEAM_POINT);
                await _hubContext.Clients.All.SendAsync("NewPoint", matchId, Actions.GUEST_TEAM_POINT);
                return;
            }
        }

        public List<string> GetKeyPhrases(List<string> words)
        {
            var dictionary = new KeyPhraseDictionary();

            using (var r = new StreamReader("key_phrase_dictionary.json"))
            {
                string json = r.ReadToEnd();
                dictionary = JsonConvert.DeserializeObject<KeyPhraseDictionary>(json);
            }

            if (dictionary is null) throw new Exception("Error while deserializing json to KeyPhraseDictionary");

            var matchedKeyPhrases = new List<string>();

            foreach(var item in dictionary.HOME_TEAM)
            {
                if (words.Contains(item))
                {
                    matchedKeyPhrases.Add(_homeTeam);
                    break;
                }
            }
            foreach (var item in dictionary.GUEST_TEAM)
            {
                if (words.Contains(item))
                {
                    matchedKeyPhrases.Add(_guestTeam);
                    break;
                }
            }
            foreach (var item in dictionary.POINT)
            {
                if (words.Contains(item))
                {
                    matchedKeyPhrases.Add(_point);
                    break;
                }
            }
            
            return matchedKeyPhrases;
        }
    }
}
