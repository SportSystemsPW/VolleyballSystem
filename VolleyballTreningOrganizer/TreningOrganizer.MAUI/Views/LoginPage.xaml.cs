namespace TreningOrganizer.MAUI.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        if (IsCredentialCorrect(Username.Text, Password.Text))
        {
            Preferences.Set("Password", Password.Text);
            Preferences.Set("Username", Username.Text);
            await Shell.Current.GoToAsync("///messageTemplates");
        }
        else
        {
            await DisplayAlert("Login failed", "Uusername or password if invalid", "Try again");
        }
    }

    private bool IsCredentialCorrect(string text1, string text2)
    {
        //TODO api call to log in
        return true;
    }
}