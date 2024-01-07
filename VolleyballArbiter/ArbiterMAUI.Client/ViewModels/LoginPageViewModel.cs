using ArbiterMAUI.Client.Services.Interfaces;
using ArbiterMAUI.Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiterMAUI.Client.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        string refereeKey;

        private readonly IAuthService _authService;

        public LoginPageViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        async Task GoToMatchList()
        {
            App._refereeKey = RefereeKey;

            var response = await _authService.CheckRefereeKey();
            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync("//MatchListPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Błąd logowania", "Został wprowadzony niepoprawny klucz sędziowski", "OK");
            }
        }
    }
}
