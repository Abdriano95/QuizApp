using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizApp.Core;
using System.Collections.ObjectModel;
using static QuizApp.Core.TriviaApiService;

namespace QuizApp.MAUI.ViewModels
{
    [ObservableObject]
    public partial class SettingsViewModel
    {
        private readonly TriviaApiService _triviaApiService;

        public SettingsViewModel(TriviaApiService triviaApiService)
        {
            _triviaApiService = triviaApiService;

            // Initialize default values
            QuestionAmounts = new ObservableCollection<int> { 5, 10, 20 };
            Difficulties = new ObservableCollection<string> { "easy", "medium", "hard" };
            Types = new ObservableCollection<string> { "multiple", "boolean" };

            // Initialize defaults for selected values
            SelectedAmount = QuestionAmounts.First();
            SelectedDifficulty = Difficulties.First();
            SelectedType = Types.First();

            // Load categories on initialization
            LoadCategoriesCommand.Execute(null);
        }

        [ObservableProperty]
        private ObservableCollection<Category> categories = new();

        [ObservableProperty]
        private Category selectedCategory = new();

        [ObservableProperty]
        private ObservableCollection<int> questionAmounts = new();

        [ObservableProperty]
        private int selectedAmount;

        [ObservableProperty]
        private ObservableCollection<string> difficulties = new();

        [ObservableProperty]
        private string selectedDifficulty = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> types = new();

        [ObservableProperty]
        private string selectedType = string.Empty;

        [RelayCommand]
        private async Task LoadCategories()
        {
            try
            {
                var categories = await _triviaApiService.GetCategoriesAsync();
                Categories = new ObservableCollection<Category>(categories ?? new List<Category>());
            }
            catch (Exception ex)
            {
                // Log error or show user-friendly message (e.g., "Failed to load categories.")
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task StartGame()
        {
            if (SelectedCategory == null || SelectedAmount == 0 || string.IsNullOrEmpty(SelectedDifficulty) || string.IsNullOrEmpty(SelectedType))
            {
                Console.WriteLine("Please make sure all settings are selected before starting the game.");
                return;
            }

            Console.WriteLine($"Navigating with Category ID: {SelectedCategory.Id}, Amount: {SelectedAmount}, Difficulty: {SelectedDifficulty}, Type: {SelectedType}");
            await Shell.Current.GoToAsync($"GamePage?category={SelectedCategory.Id}&amount={SelectedAmount}&difficulty={SelectedDifficulty}&type={SelectedType}");
        }



        partial void OnSelectedCategoryChanged(Category value)
        {
            Console.WriteLine($"SelectedCategory: {value?.Name}");
        }

        partial void OnSelectedAmountChanged(int value)
        {
            Console.WriteLine($"SelectedAmount: {value}");
        }

        partial void OnSelectedDifficultyChanged(string value)
        {
            Console.WriteLine($"SelectedDifficulty: {value}");
        }

        partial void OnSelectedTypeChanged(string value)
        {
            Console.WriteLine($"SelectedType: {value}");
        }

    }
}
