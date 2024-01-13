using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoginViewModel loginViewModel)
	{
        BindingContext = loginViewModel;
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        Application.Current.Quit();
        return true;
    }
}