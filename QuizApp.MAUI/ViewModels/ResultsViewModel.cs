using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizApp.MAUI.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace QuizApp.MAUI.ViewModels
{
    [ObservableObject]
    [QueryProperty(nameof(TotalQuestions), "total")]
    [QueryProperty(nameof(CorrectAnswers), "score")]
    [QueryProperty(nameof(SerializedResults), "results")]
    public partial class ResultsViewModel
    {
        [ObservableProperty]
        private int totalQuestions;

        [ObservableProperty]
        private int correctAnswers;

        [ObservableProperty]
        private ObservableCollection<Result> results = new();

        private string serializedResults = string.Empty;
        public string SerializedResults
        {
            get => serializedResults;
            set
            {
                SetProperty(ref serializedResults, value);

                // Deserialize the results
                var deserializedResults = JsonConvert.DeserializeObject<ObservableCollection<Result>>(serializedResults);
                if (deserializedResults != null)
                {
                    Results = deserializedResults;
                }
            }
        }

        [ObservableProperty]
        private string scoreText = string.Empty;

        // Constructor
        public ResultsViewModel()
        {
            UpdateScoreText();
        }


        partial void OnTotalQuestionsChanged(int value)
        {
            Console.WriteLine($"TotalQuestions set to: {value}");
            UpdateScoreText();
        }

        partial void OnCorrectAnswersChanged(int value)
        {
            Console.WriteLine($"CorrectAnswers set to: {value}");
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            ScoreText = $"You answered {CorrectAnswers} out of {TotalQuestions} questions correctly!";
        }


        [RelayCommand]
        private async Task PlayAgain()
        {
            //Reset current parameters 
            TotalQuestions = 0;
            CorrectAnswers = 0;
            Results.Clear();


            // navigate to the game page
            await Shell.Current.GoToAsync("//SettingsPage");
        }

        // Quit the game
        [RelayCommand]
        private void QuitGame()
        {
#if ANDROID
            //Reset current parameters 
            TotalQuestions = 0;
            CorrectAnswers = 0;
            Results.Clear();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif IOS
    throw new PlatformNotSupportedException("Closing the app programmatically is not supported on iOS.");
#elif WINDOWS
    Application.Current.Quit();
#endif
        }

        [RelayCommand]
        private async Task NavigateToWelcome()
        {
            await Shell.Current.GoToAsync("//WelcomePage");
        }

    }
}
