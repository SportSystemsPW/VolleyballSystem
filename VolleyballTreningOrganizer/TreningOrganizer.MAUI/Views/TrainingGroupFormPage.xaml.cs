using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class TrainingGroupFormPage : ContentPage
{
	public TrainingGroupFormPage(TrainingGroupFormViewModel trainingGroupFormViewModel)
	{
		BindingContext = trainingGroupFormViewModel;
		InitializeComponent();
	}
}