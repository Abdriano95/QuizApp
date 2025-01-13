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
    public partial class HighscoreViewModel
    {
        private readonly HighScoreResultService _highScoreResultService;

        [ObservableProperty]
        private ObservableCollection<HighScoreResult> _results;
        public HighscoreViewModel(HighScoreResultService highScoreResultService)
        {
            _highScoreResultService = highScoreResultService;
            _ = LoadResults();
        }
        private async Task  LoadResults()
        {
            var results = await _highScoreResultService.GetResultsAsync();
            Results = new ObservableCollection<HighScoreResult>(results);
        }


        [RelayCommand]
        private async Task NavigateToWelcome()
        {
            await Shell.Current.GoToAsync("//WelcomePage");
        }
    }
}
