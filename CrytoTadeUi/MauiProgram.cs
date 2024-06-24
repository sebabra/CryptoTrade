using CommunityToolkit.Maui;
using CrytoTadeUi.Services;
using CrytoTadeUi.Services.CryptoTadeApi;
using CrytoTadeUi.ViewModels;
using CrytoTadeUi.Views;
using Microsoft.Extensions.Logging;

namespace CrytoTadeUi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // Enregistrement de HttpClient comme singleton

            builder.Services.AddSingleton<HttpClient>(sp =>
            {
                var client = new HttpClient { BaseAddress = new Uri("https://localhost:7240/api/") };
                // Configurez ici d'autres paramètres si nécessaire
                return client;
            });

            builder.Services.AddSingleton<ApiService>();

            builder.Services.AddSingleton<ExchangeInfoService>();
            builder.Services.AddSingleton<LoadCandles>();
            builder.Services.AddSingleton<LoadCandlesViewModel>();

            return builder.Build();
        }
    }
}
