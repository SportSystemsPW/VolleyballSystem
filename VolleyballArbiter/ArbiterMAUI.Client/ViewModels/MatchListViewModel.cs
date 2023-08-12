using ArbiterMAUI.Client.Models;
using ArbiterMAUI.Client.Services.Interfaces;
using ArbiterMAUI.Client.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ArbiterMAUI.Client.ViewModels
{
    public partial class MatchListViewModel : BaseViewModel
    {
        private readonly IMatchService _matchService;
        public ObservableCollection<Match> Matches { get; } = new();

        public MatchListViewModel(IMatchService matchService) 
        {
            Title = "Lista meczów";
            _matchService = matchService;
        }

        [RelayCommand]
        async Task GetMatchesAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet Issue",
                        $"Check your internet and try again!", "Ok");
                    return;
                }

                IsBusy = true;
                var matches = new ObservableCollection<Match>(await _matchService.DownloadMatches());

                if (Matches.Count != 0)
                    Matches.Clear();

                foreach (var match in matches)
                {
                    Matches.Add(match);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Unable to get matches: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToMatchDetailsAsync(Match match)
        {
            await Shell.Current.GoToAsync($"{nameof(MatchRecordPage)}", true,
                new Dictionary<string, object>
                {
                    { "MatchDetails", match }
                });
        }
    }
}
