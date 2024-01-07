using ArbiterMAUI.Client.Models;
using ArbiterMAUI.Client.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using System.Globalization;

namespace ArbiterMAUI.Client.ViewModels
{
    [QueryProperty("MatchDetails", "MatchDetails")]
    public partial class MatchRecordViewModel : BaseViewModel
    {
        private readonly ISpeechToTextService _speechToText;
        private readonly ISignalrTextAnalyzerService _signalrTextAnalyzer;
        private readonly IServiceBusService _serviceBus;
        private readonly IMatchService _matchService;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public MatchRecordViewModel(ISpeechToTextService speechToText, ISignalrTextAnalyzerService signalrTextAnalyzer, IServiceBusService serviceBus, IMatchService matchService)
        {
            _speechToText = speechToText;
            _signalrTextAnalyzer = signalrTextAnalyzer;
            _serviceBus = serviceBus;
            _matchService = matchService;

            Set1Color = Color.FromArgb("#9292cb");
            Set2Color = Color.FromArgb("#4a4a4b");
            Set3Color = Color.FromArgb("#4a4a4b");
            Set4Color = Color.FromArgb("#4a4a4b");
            Set5Color = Color.FromArgb("#4a4a4b");

            MatchHasNotStarted = false;
            MatchEnded = false;
            MatchInProgress = false;
            Set1Selected = true;
            Set2Selected = false;
            Set3Selected = false;
            Set4Selected = false;
            Set5Selected = false;
        }

        partial void OnMatchDetailsChanged(Match value)
        {
            Title = value.HomeTeam + " - " + value.GuestTeam;
            var matchStatus = Task.Run<int>(async () => await _matchService.GetMatchStatus(value.Id)).Result;

            if (matchStatus == 0) MatchHasNotStarted = true;
            else if (matchStatus == 1) MatchInProgress = true;
            else if (matchStatus == 2) MatchEnded = true;

            Task.Run(async () => await RefreshSetsStatuses());
        }

        [RelayCommand]
        void GoToSet1Details()
        {
            Set1Color = Color.FromArgb("#9292cb");
            Set2Color = Color.FromArgb("#4a4a4b");
            Set3Color = Color.FromArgb("#4a4a4b");
            Set4Color = Color.FromArgb("#4a4a4b");
            Set5Color = Color.FromArgb("#4a4a4b");
            Set1Selected = true;
            Set2Selected = false;
            Set3Selected = false;
            Set4Selected = false;
            Set5Selected = false;
        }

        [RelayCommand]
        void GoToSet2Details()
        {
            Set1Color = Color.FromArgb("#4a4a4b");
            Set2Color = Color.FromArgb("#9292cb");
            Set3Color = Color.FromArgb("#4a4a4b");
            Set4Color = Color.FromArgb("#4a4a4b");
            Set5Color = Color.FromArgb("#4a4a4b");
            Set1Selected = false;
            Set2Selected = true;
            Set3Selected = false;
            Set4Selected = false;
            Set5Selected = false;
        }

        [RelayCommand]
        void GoToSet3Details()
        {
            Set1Color = Color.FromArgb("#4a4a4b");
            Set2Color = Color.FromArgb("#4a4a4b");
            Set3Color = Color.FromArgb("#9292cb");
            Set4Color = Color.FromArgb("#4a4a4b");
            Set5Color = Color.FromArgb("#4a4a4b");
            Set1Selected = false;
            Set2Selected = false;
            Set3Selected = true;
            Set4Selected = false;
            Set5Selected = false;
        }

        [RelayCommand]
        void GoToSet4Details()
        {
            Set1Color = Color.FromArgb("#4a4a4b");
            Set2Color = Color.FromArgb("#4a4a4b");
            Set3Color = Color.FromArgb("#4a4a4b");
            Set4Color = Color.FromArgb("#9292cb");
            Set5Color = Color.FromArgb("#4a4a4b");
            Set1Selected = false;
            Set2Selected = false;
            Set3Selected = false;
            Set4Selected = true;
            Set5Selected = false;
        }

        [RelayCommand]
        void GoToSet5Details()
        {
            Set1Color = Color.FromArgb("#4a4a4b");
            Set2Color = Color.FromArgb("#4a4a4b");
            Set3Color = Color.FromArgb("#4a4a4b");
            Set4Color = Color.FromArgb("#4a4a4b");
            Set5Color = Color.FromArgb("#9292cb");
            Set1Selected = false;
            Set2Selected = false;
            Set3Selected = false;
            Set4Selected = false;
            Set5Selected = true;
        }

        [RelayCommand]
        async Task StartMatch()
        {
            var started = await _matchService.StartMatch(MatchDetails.Id);

            if (started)
            {
                MatchHasNotStarted = false;
                MatchInProgress = true;
            }
        }

        [RelayCommand]
        async Task ListenAsync()
        {
            IsBusy = true;
            cancellationTokenSource = new CancellationTokenSource();
            var isAuthorized = await _speechToText.RequestPermissions();
            if (isAuthorized)
            {
                try
                {
                    await _speechToText.Listen(CultureInfo.GetCultureInfo("pl-PL"),
                        new Progress<string>(partialText =>
                        {
                            if (Set1Selected)
                            {
                                Set1TextFromSpeech += partialText + "\n";
                                _serviceBus.SendMessage(MatchDetails.Id + ". " + partialText);
                                OnPropertyChanged(nameof(Set1TextFromSpeech));
                            }
                            else if (Set2Selected)
                            {
                                Set2TextFromSpeech += partialText + "\n";
                                _serviceBus.SendMessage(MatchDetails.Id + ". " + partialText);
                                OnPropertyChanged(nameof(Set2TextFromSpeech));
                            }
                            else if (Set3Selected)
                            {
                                Set3TextFromSpeech += partialText + "\n";
                                _serviceBus.SendMessage(MatchDetails.Id + ". " + partialText);
                                OnPropertyChanged(nameof(Set3TextFromSpeech));
                            }
                            else if (Set4Selected)
                            {
                                Set4TextFromSpeech += partialText + "\n";
                                _serviceBus.SendMessage(MatchDetails.Id + ". " + partialText);
                                OnPropertyChanged(nameof(Set4TextFromSpeech));
                            }
                            else if (Set5Selected)
                            {
                                Set5TextFromSpeech += partialText + "\n";
                                _serviceBus.SendMessage(MatchDetails.Id + ". " + partialText);
                                OnPropertyChanged(nameof(Set5TextFromSpeech));
                            }
                        }), cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                }
            }
            else
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Permission Error", "No microphone access", "OK");
            }
            IsBusy = false;
        }

        [RelayCommand]
        void ListenCancel()
        {
            cancellationTokenSource?.Cancel();
        }
        
        [RelayCommand]
        async Task StartSet()
        {
            if (Set1Selected)
            {
                var result = await _matchService.StartSet(MatchDetails.Id, 1);
            }
            else if (Set2Selected)
            {
                var result = await _matchService.StartSet(MatchDetails.Id, 2);
            }
            else if (Set3Selected)
            {
                var result = await _matchService.StartSet(MatchDetails.Id, 3);
            }
            else if (Set4Selected)
            {
                var result = await _matchService.StartSet(MatchDetails.Id, 4);
            }
            else if (Set5Selected)
            {
                var result = await _matchService.StartSet(MatchDetails.Id, 5);
            }

            await RefreshSetsStatuses();
        }

        [RelayCommand]
        async Task FinishMatch()
        {
            var result = await _matchService.FinishMatch(MatchDetails.Id);

            MatchInProgress = false;
            Set1Selected = false;
            Set2Selected = false;
            Set3Selected = false;
            Set4Selected = false;
            Set5Selected = false;
            MatchEnded = true;
        }

        [RelayCommand]
        async Task FinishSet()
        {
            ListenCancel();
            if (Set1Selected)
            {
                var result = await _matchService.FinishSet(MatchDetails.Id, 1);
            }
            else if (Set2Selected)
            {
                var result = await _matchService.FinishSet(MatchDetails.Id, 2);
            }
            else if (Set3Selected)
            {
                var result = await _matchService.FinishSet(MatchDetails.Id, 3);
            }
            else if (Set4Selected)
            {
                var result = await _matchService.FinishSet(MatchDetails.Id, 4);
            }
            else if (Set5Selected)
            {
                var result = await _matchService.FinishSet(MatchDetails.Id, 5);
            }

            await RefreshSetsStatuses();
        }

        async Task RefreshSetsStatuses()
        {
            // 0 - not started
            // 1 - in progress
            // 2 - finished
            int set1status = await _matchService.GetSetStatus(MatchDetails.Id, 1);
            int set2status = await _matchService.GetSetStatus(MatchDetails.Id, 2);
            int set3status = await _matchService.GetSetStatus(MatchDetails.Id, 3);
            int set4status = await _matchService.GetSetStatus(MatchDetails.Id, 4);
            int set5status = await _matchService.GetSetStatus(MatchDetails.Id, 5);

            Set1CannotStart = false;
            Set1Finished = false;
            Set1InProgress = false;
            Set1NotStarted = false;
            Set2CannotStart = false;
            Set2Finished = false;
            Set2InProgress = false;
            Set2NotStarted = false;
            Set3CannotStart = false;
            Set3Finished = false;
            Set3InProgress = false;
            Set3NotStarted = false;
            Set4CannotStart = false;
            Set4Finished = false;
            Set4InProgress = false;
            Set4NotStarted = false;
            Set5CannotStart = false;
            Set5Finished = false;
            Set5InProgress = false;
            Set5NotStarted = false;

            if(set1status == 0)
            {
                Set1NotStarted = true;
            }
            else if(set1status == 1)
            {
                Set1InProgress = true;
            }
            else if(set1status == 2)
            {
                Set1Finished = true;
            }

            if (set2status == 0 && set1status == 2)
            {
                Set2NotStarted = true;
            }
            else if (set2status == 0 && set1status != 2)
            {
                Set2CannotStart = true;
            }
            else if (set2status == 1)
            {
                Set2InProgress = true;
            }
            else if (set2status == 2)
            {
                Set2Finished = true;
            }

            if (set3status == 0 && set2status == 2)
            {
                Set3NotStarted = true;
            }
            else if (set3status == 0 && set2status != 2)
            {
                Set3CannotStart = true;
            }
            else if (set3status == 1)
            {
                Set3InProgress = true;
            }
            else if (set3status == 2)
            {
                Set3Finished = true;
            }

            if (set4status == 0 && set3status == 2)
            {
                Set4NotStarted = true;
            }
            else if (set4status == 0 && set3status != 2)
            {
                Set4CannotStart = true;
            }
            else if (set4status == 1)
            {
                Set4InProgress = true;
            }
            else if (set4status == 2)
            {
                Set4Finished = true;
            }

            if (set5status == 0 && set4status == 2)
            {
                Set5NotStarted = true;
            }
            else if (set5status == 0 && set4status != 2)
            {
                Set5CannotStart = true;
            }
            else if (set5status == 1)
            {
                Set5InProgress = true;
            }
            else if (set5status == 2)
            {
                Set5Finished = true;
            }
        }

        [ObservableProperty]
        bool matchHasNotStarted;

        [ObservableProperty]
        bool matchInProgress;

        [ObservableProperty]
        bool matchEnded;

        [ObservableProperty]
        bool set1Selected;

        [ObservableProperty]
        bool set2Selected;

        [ObservableProperty]
        bool set3Selected;

        [ObservableProperty]
        bool set4Selected;

        [ObservableProperty]
        bool set5Selected;

        [ObservableProperty]
        bool set1CannotStart;

        [ObservableProperty]
        bool set1NotStarted;

        [ObservableProperty]
        bool set1Finished;

        [ObservableProperty]
        bool set1InProgress;

        [ObservableProperty]
        bool set2CannotStart;

        [ObservableProperty]
        bool set2NotStarted;

        [ObservableProperty]
        bool set2Finished;

        [ObservableProperty]
        bool set2InProgress;

        [ObservableProperty]
        bool set3CannotStart;

        [ObservableProperty]
        bool set3NotStarted;

        [ObservableProperty]
        bool set3Finished;

        [ObservableProperty]
        bool set3InProgress;

        [ObservableProperty]
        bool set4CannotStart;

        [ObservableProperty]
        bool set4NotStarted;

        [ObservableProperty]
        bool set4Finished;

        [ObservableProperty]
        bool set4InProgress;

        [ObservableProperty]
        bool set5CannotStart;

        [ObservableProperty]
        bool set5NotStarted;

        [ObservableProperty]
        bool set5Finished;

        [ObservableProperty]
        bool set5InProgress;

        [ObservableProperty]
        Color set1Color;

        [ObservableProperty]
        Color set2Color;

        [ObservableProperty]
        Color set3Color;

        [ObservableProperty]
        Color set4Color;

        [ObservableProperty]
        Color set5Color;

        [ObservableProperty]
        Match matchDetails;

        [ObservableProperty]
        string set1TextFromSpeech;

        [ObservableProperty]
        string set2TextFromSpeech;

        [ObservableProperty]
        string set3TextFromSpeech;

        [ObservableProperty]
        string set4TextFromSpeech;

        [ObservableProperty]
        string set5TextFromSpeech;
    }
}
