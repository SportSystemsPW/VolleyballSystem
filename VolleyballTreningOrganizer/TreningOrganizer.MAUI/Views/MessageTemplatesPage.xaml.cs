using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class MessageTemplatesPage : ContentPage
{
    
    public MessageTemplatesPage(MessageTemplatesViewModel messageTemplatesViewModel)
	{
        BindingContext = messageTemplatesViewModel;
        InitializeComponent();
    }
}