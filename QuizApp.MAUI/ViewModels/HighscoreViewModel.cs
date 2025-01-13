using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizApp.MAUI.Models;
using QuizApp.MAUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.MAUI.ViewModels
{
    [ObservableObject]
    public partial class HighscoreViewModel : IQueryAttributable
    {
        private readonly HighScoreResultService _highScoreResultService;

        [ObservableProperty]
        private ObservableCollection<HighScoreResult> _results;
        public HighscoreViewModel(HighScoreResultService highScoreResultService)
        {
            _highScoreResultService = highScoreResultService;
            LoadResults();
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await LoadResults();
        }
        private async Task  LoadResults()
        {
            Console.WriteLine("Loading highscores...");
            var results = await _highScoreResultService.GetResultsAsync();
            foreach ( var result in results )
            {
                Console.WriteLine($"Player: {result.PlayerName}, Score: {result.Score}");
            }
            Results = new ObservableCollection<HighScoreResult>(results);
        }


        [RelayCommand]
        private async Task NavigateToWelcome()
        {
            await Shell.Current.GoToAsync("//WelcomePage");
        }
    }
}
