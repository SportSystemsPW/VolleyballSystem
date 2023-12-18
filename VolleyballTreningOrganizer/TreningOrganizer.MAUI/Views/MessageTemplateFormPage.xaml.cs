using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class MessageTemplateFormPage : ContentPage
{
	public MessageTemplateFormPage(MessageTemplateFormViewModel messageTemplateFormViewModel)
	{
		BindingContext = messageTemplateFormViewModel;
		InitializeComponent();
	}
}