using ArbiterMAUI.Client.Views;

namespace ArbiterMAUI.Client;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(MatchListPage), typeof(MatchListPage));
    }
}
