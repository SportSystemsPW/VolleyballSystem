using ArbiterMAUI.Client.ViewModels;

namespace ArbiterMAUI.Client.Views;

public partial class FilterPage : ContentPage
{
	public FilterPage(FilterPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}