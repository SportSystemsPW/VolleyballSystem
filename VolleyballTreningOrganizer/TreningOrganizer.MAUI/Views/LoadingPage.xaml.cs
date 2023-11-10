namespace TreningOrganizer.MAUI.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (await isAuthenticated())
        {
            await Shell.Current.GoToAsync("///templateTab");
        }
        else
        {
            await Shell.Current.GoToAsync("login");
        }
        base.OnNavigatedTo(args);
    }

    async Task<bool> isAuthenticated()
    {
        string password = Preferences.Get("Password", string.Empty);
        string username = Preferences.Get("Username", string.Empty);
        if(password == string.Empty || username == string.Empty)
        {
            return false;
        }
        //TODO api call to get jwt and save jwt
        await Task.Delay(2000);
        return true;
    }

    protected override bool OnBackButtonPressed()
    {
        Application.Current.Quit();
        return true;
    }
}