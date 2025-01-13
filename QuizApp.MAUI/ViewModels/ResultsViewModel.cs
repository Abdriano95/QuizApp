using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace QuizApp.MAUI.ViewModels
{
    [ObservableObject]
    public partial class ResultsViewModel
    {


        [RelayCommand]
        private async Task PlayAgain()
        {
            // navigate to the game page
            await Shell.Current.GoToAsync("//SettingsPage");
        }

    }
}
