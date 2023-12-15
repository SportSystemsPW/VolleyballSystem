using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TreningOrganizer.MAUI.Models;
using TreningOrganizer.MAUI.ViewModels;
using TreningOrganizer.MAUI.Views;

namespace TreningOrganizer.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient("HttpClient", client =>
            {
                //"https://10.0.2.2:44441/api/"
                client.BaseAddress = new Uri("https://10.0.2.2:44441/api/");
            });

            builder.Services.AddSingleton<MessageTemplatesPage>();
            builder.Services.AddSingleton<MessageTemplatesViewModel>();

            builder.Services.AddSingleton<MessageTemplateFormPage>();
            builder.Services.AddSingleton<MessageTemplateFormViewModel>();

            builder.Services.AddSingleton<PaymentPage>();
            builder.Services.AddSingleton<PaymentsViewModel>();

            builder.Services.AddSingleton<TrainingsPage>();
            builder.Services.AddSingleton<TrainingsViewModel>();

            builder.Services.AddSingleton<TrainingDetailsPage>();
            builder.Services.AddSingleton<TrainingFormPage>();
            builder.Services.AddSingleton<TrainingFormViewModel>();

            builder.Services.AddSingleton<TrainingGroupsPage>();
            builder.Services.AddSingleton<TrainingGroupsViewModel>();

            builder.Services.AddSingleton<TrainingGroupFormPage>();
            builder.Services.AddSingleton<TrainingGroupFormViewModel>();

            return builder.Build();
        }
    }
}