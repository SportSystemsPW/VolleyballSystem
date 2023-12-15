using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class TrainingsPage : ContentPage
{
	public TrainingsPage(TrainingsViewModel trainingsViewModel)
	{
		BindingContext = trainingsViewModel;
		InitializeComponent();
	}
}