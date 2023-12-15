using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class TrainingDetailsPage : ContentPage
{
	public TrainingDetailsPage(TrainingFormViewModel trainingFormViewModel)
	{
		BindingContext = trainingFormViewModel;
		InitializeComponent();
	}
}