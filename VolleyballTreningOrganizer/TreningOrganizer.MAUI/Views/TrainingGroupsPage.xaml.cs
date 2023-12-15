using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class TrainingGroupsPage : ContentPage
{
	public TrainingGroupsPage(TrainingGroupsViewModel trainingGroupsViewModel)
	{
		BindingContext = trainingGroupsViewModel;
		InitializeComponent();
	}
}