using ArbiterMAUI.Client.Platforms.Services;
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
		builder.Services.AddTransient<ISignalrTextAnalyzerService, SignalrTextAnalyzerService>();
		builder.Services.AddTransient<IServiceBusService, ServiceBusService>();
		//builder.Services.AddTransient<ISpeechToTextService, MauiSpeechToText>();
		builder.Services.AddTransient<ISpeechToTextService, AzureSpeechToText>();
		builder.Services.AddTransient<IAuthService, AuthService>();

        builder.Services.AddTransient<MatchListViewModel>();
        builder.Services.AddTransient<MatchRecordViewModel>();
        builder.Services.AddTransient<LoginPageViewModel>();
        builder.Services.AddTransient<FilterPageViewModel>();

        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddTransient<MatchListPage>(); 
		builder.Services.AddTransient<MatchRecordPage>();
		builder.Services.AddTransient<FilterPage>();

        return builder.Build();
	}
}
