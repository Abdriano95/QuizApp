using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.MAUI.ViewModels
{
    [ObservableObject]
    public partial class WelcomeViewModel
    {
        [RelayCommand]
        private async Task NavigateToSettings()
        {
            await Shell.Current.GoToAsync("//SettingsPage");
        }

        [RelayCommand]
        private async Task NavigateToHighscore()
        {
            await Shell.Current.GoToAsync("//HighscorePage");
        }
    }
}
