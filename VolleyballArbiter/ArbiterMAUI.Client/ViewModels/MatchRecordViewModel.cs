using ArbiterMAUI.Client.Models;
using ArbiterMAUI.Client.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;

namespace ArbiterMAUI.Client.ViewModels
{
    [QueryProperty("MatchDetails", "MatchDetails")]
    public partial class MatchRecordViewModel : BaseViewModel
    {
        private readonly ISpeechToTextService _speechToText;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public MatchRecordViewModel(ISpeechToTextService speechToText)
        {
            _speechToText = speechToText;
        }
        
        [ObservableProperty]
        Match matchDetails;

        [ObservableProperty]
        string textFromSpeech;

        [RelayCommand]
        async Task ListenAsync()
        {
            IsBusy = true;
            var isAuthorized = await _speechToText.RequestPermissions();
            if (isAuthorized)
            {
                try
                {
                    TextFromSpeech = await _speechToText.Listen(CultureInfo.GetCultureInfo("pl-PL"),
                        new Progress<string>(partialText =>
                        {
                            TextFromSpeech += partialText + "\n";
                            OnPropertyChanged(nameof(TextFromSpeech));
                            // call signalR and send sentence to backend
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
    }
}
