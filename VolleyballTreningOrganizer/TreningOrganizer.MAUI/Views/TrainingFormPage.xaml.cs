using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class TrainingFormPage : ContentPage
{
	public TrainingFormPage(TrainingFormViewModel trainingFormViewModel)
	{
		BindingContext = trainingFormViewModel;
		InitializeComponent();
	}
}