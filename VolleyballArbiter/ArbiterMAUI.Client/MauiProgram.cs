using ArbiterMAUI.Client.Services;
using ArbiterMAUI.Client.Services.Interfaces;
using ArbiterMAUI.Client.ViewModels;
using ArbiterMAUI.Client.Views;
using Microsoft.Extensions.Logging;

namespace ArbiterMAUI.Client;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddTransient<IMatchService, MatchService>();
        builder.Services.AddSingleton<MatchListViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MatchListPage>();

        return builder.Build();
	}
}
