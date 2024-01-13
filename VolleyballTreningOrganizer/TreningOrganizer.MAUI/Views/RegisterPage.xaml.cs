using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(LoginViewModel loginViewModel)
	{
		BindingContext = loginViewModel;
		InitializeComponent();
	}
}