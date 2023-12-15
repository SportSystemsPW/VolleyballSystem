using TreningOrganizer.MAUI.ViewModels;

namespace TreningOrganizer.MAUI.Views;

public partial class PaymentPage : ContentPage
{
	public PaymentPage(PaymentsViewModel paymentsViewModel)
	{
		BindingContext = paymentsViewModel;
		InitializeComponent();
	}
}