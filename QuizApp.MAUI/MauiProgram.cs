using Microsoft.Extensions.Logging;
using QuizApp.Core;
using QuizApp.MAUI.ViewModels;
using QuizApp.MAUI.Views;

namespace QuizApp.MAUI
{
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
            // Add services
            builder.Services.AddTransient<TriviaApiService>();
            // Add pages
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<ResultsPage>();
            builder.Services.AddTransient<GamePage>();

            // Add view models
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<ResultsViewModel>();
            builder.Services.AddTransient<GameViewModel>();

            return builder.Build();
        }
    }
}
