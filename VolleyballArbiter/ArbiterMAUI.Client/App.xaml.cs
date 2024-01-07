namespace ArbiterMAUI.Client;

public partial class App : Application
{
	public static DateTime? _fromDateSearch { get; set; }
	public static DateTime? _toDateSearch { get; set; }
	public static string _refereeKey { get; set; }

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
