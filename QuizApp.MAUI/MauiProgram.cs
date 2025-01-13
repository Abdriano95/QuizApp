using Microsoft.Extensions.Logging;
using QuizApp.Core;
using QuizApp.MAUI.Services;
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
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "QuizApp.db");
            // Add services
            builder.Services.AddTransient<TriviaApiService>();
            builder.Services.AddTransient<HighScoreResultService>(provider =>
                                            new HighScoreResultService(dbPath));


            // Add pages
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<ResultsPage>();
            builder.Services.AddTransient<GamePage>();
            builder.Services.AddTransient<WelcomePage>();
            builder.Services.AddTransient<HighscorePage>();

            // Add view models
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<ResultsViewModel>();
            builder.Services.AddTransient<GameViewModel>();
            builder.Services.AddTransient<WelcomeViewModel>();
            builder.Services.AddTransient<HighscoreViewModel>();

            return builder.Build();
        }
    }
}
