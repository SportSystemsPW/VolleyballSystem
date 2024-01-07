using ArbiterMAUI.Client.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace ArbiterMAUI.Client.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}

