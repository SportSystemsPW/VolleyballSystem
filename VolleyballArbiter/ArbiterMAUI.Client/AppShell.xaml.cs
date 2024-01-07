using ArbiterMAUI.Client.Views;

namespace ArbiterMAUI.Client;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(MatchListPage), typeof(MatchListPage));
        Routing.RegisterRoute(nameof(MatchRecordPage), typeof(MatchRecordPage));
        Routing.RegisterRoute(nameof(FilterPage), typeof(FilterPage));
    }
}
