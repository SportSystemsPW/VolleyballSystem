using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Input;
using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.MAUI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string Email { get; set; } = "email@email.com";
        public string Password { get; set; } = "haslohaslo";
        public string RepeatPassword { get; set; }
        public ICommand LoginCommand
        {
            get
            {
                return new Command(Login);
            }
        }
        public ICommand RegisterCommand
        {
            get
            {
                return new Command(Register);
            }
        }
        public ICommand GoToRegisterCommand
        {
            get
            {
                return new Command(GoToRegister);
            }
        }
        public ICommand GoToLoginCommand
        {
            get
            {
                return new Command(GoToLogin);
            }
        }
        public ICommand OnLoadingPageAppearCommand
        {
            get
            {
                return new Command(LoadingPageAppear);
            }
        }
        public LoginViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async void Login()
        {
            try
            {
                string jwt = await TryToLogin(Email, Password);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwt);
                Preferences.Set("Password", Password);
                Preferences.Set("Username", Email);
                await Shell.Current.GoToAsync("///templateTab");
            }
            catch { }
        }

        private async void Register()
        {
            bool isEmailInCorrectFormat = true;
            try
            {
                new MailAddress(Email);
            }
            catch
            {
                isEmailInCorrectFormat = false;
            }

            bool isPasswordCorrectLength = Password.Length > 8;
            bool doPasswordsMatch = Password == RepeatPassword;

            List<string> errors = new List<string>();
            if (!isEmailInCorrectFormat)
                errors.Add("Incorrect email format");
            if (!isPasswordCorrectLength)
                errors.Add("Password must be at least 8 characters long");
            if(!doPasswordsMatch)
                errors.Add("Passwords doesen't match");

            if(errors.Count > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", string.Join('\n', errors), "OK");
                return;
            }

            try
            {
                TrainerDTO newUser = new TrainerDTO()
                {
                    Email = Email,
                    Password = Password
                };
                await PostRequest<bool>("User/register", newUser);
                await Shell.Current.GoToAsync("///login");
            }
            catch { }

        }

        private async void GoToRegister()
        {
            await Shell.Current.GoToAsync("///register");
        }

        private async void GoToLogin()
        {
            await Shell.Current.GoToAsync("///login");
        }

        private async void LoadingPageAppear()
        {
            string password = Preferences.Get("Password", string.Empty);
            string username = Preferences.Get("Username", string.Empty);
            if(password != string.Empty && username != string.Empty)
            {
                try
                {
                    string jwt = await TryToLogin(password, username);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwt);
                    await Shell.Current.GoToAsync("///templateTab");
                }
                catch 
                {
                    Preferences.Set("Password", string.Empty);
                    Preferences.Set("Username", string.Empty);
                    await Shell.Current.GoToAsync("///login");
                }
            }
            else
            {
                await Shell.Current.GoToAsync("///login");
            }
        }

        private async Task<string> TryToLogin(string email, string password)
        {
            TrainerDTO user = new TrainerDTO()
            {
                Email = Email,
                Password = Password
            };
            return await PostRequest<string>("User/login", user);
        }
    }
}
