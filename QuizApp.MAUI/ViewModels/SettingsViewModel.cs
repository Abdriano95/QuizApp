using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizApp.Core;
using QuizApp.MAUI.Helpers;
using System.Collections.ObjectModel;
using static QuizApp.Core.TriviaApiService;

namespace QuizApp.MAUI.ViewModels
{
    [ObservableObject]
    public partial class SettingsViewModel
    {

        private readonly List<ValidCombination> _validCombinations;

        public SettingsViewModel()
        {
            // Load valid combinations from CSV
            _validCombinations = CsvLoader.LoadValidCombinations();

            // Initialize the lists based on the valid combinations
            Categories = new ObservableCollection<string>(_validCombinations.Select(c => c.CategoryName).Distinct());
            SelectedCategory = Categories.FirstOrDefault();

            QuestionAmounts = new ObservableCollection<int>();
            Difficulties = new ObservableCollection<string>();
            Types = new ObservableCollection<string>();
        }

        [ObservableProperty]
        private ObservableCollection<string> _categories;

        [ObservableProperty]
        private string _selectedCategory;

        [ObservableProperty]
        private ObservableCollection<int> _questionAmounts;

        [ObservableProperty]
        private int _selectedAmount;

        [ObservableProperty]
        private ObservableCollection<string> _difficulties;

        [ObservableProperty]
        private string _selectedDifficulty;

        [ObservableProperty]
        private ObservableCollection<string> _types;

        [ObservableProperty]
        private string _selectedType;

        partial void OnSelectedCategoryChanged(string value)
        {
            // Filtrera kombinationer baserat på vald kategori
            var filteredCombinations = _validCombinations.Where(c => c.CategoryName == value).ToList();

            if (filteredCombinations.Any())
            {
                // Uppdatera antal frågor
                QuestionAmounts = new ObservableCollection<int>(filteredCombinations.Select(c => c.Amount).Distinct());
                SelectedAmount = QuestionAmounts.FirstOrDefault();

                // Uppdatera svårighetsgrader
                Difficulties = new ObservableCollection<string>(filteredCombinations.Select(c => c.Difficulty).Distinct());
                SelectedDifficulty = Difficulties.FirstOrDefault();

                // Uppdatera typer
                Types = new ObservableCollection<string>(filteredCombinations.Select(c => c.Type).Distinct());
                SelectedType = Types.FirstOrDefault();
            }
            else
            {
                // Om inga kombinationer finns, töm alternativen
                QuestionAmounts = new ObservableCollection<int>();
                Difficulties = new ObservableCollection<string>();
                Types = new ObservableCollection<string>();

                SelectedAmount = 0;
                SelectedDifficulty = string.Empty;
                SelectedType = string.Empty;

                Shell.Current.DisplayAlert("No Questions Available",
                    $"Unfortunately, there are no available questions for the category \"{value}\".\n\n" +
                    "Please choose a different category.",
                    "OK");

                Console.WriteLine("No valid combinations for the selected category.");
            }
        }



        [RelayCommand]
        private async Task StartGame()
        {
            if (string.IsNullOrEmpty(SelectedCategory) || SelectedAmount == 0 || string.IsNullOrEmpty(SelectedDifficulty) || string.IsNullOrEmpty(SelectedType))
            {
                await Shell.Current.DisplayAlert("Missing Settings", "Please ensure all settings are selected before starting the game.", "OK");
                return;
            }

            // Checks if the selected combination is valid
            var isValidCombination = _validCombinations.Any(c =>
                c.CategoryName == SelectedCategory &&
                c.Amount == SelectedAmount &&
                c.Difficulty == SelectedDifficulty &&
                c.Type == SelectedType);

            if (!isValidCombination)
            {
                await Shell.Current.DisplayAlert("No Questions Available",
                    $"Unfortunately, there are no questions available for the current settings:\n\n" +
                    $"- Category: {SelectedCategory}\n" +
                    $"- Amount: {SelectedAmount}\n" +
                    $"- Difficulty: {SelectedDifficulty}\n" +
                    $"- Type: {SelectedType}\n\n" +
                    $"Please try different settings.",
                    "OK");
                return;
            }

            // Get the category ID based on the selected category
            var categoryId = _validCombinations.First(c => c.CategoryName == SelectedCategory).CategoryId;

            Console.WriteLine($"Navigating with Category ID: {categoryId}, Amount: {SelectedAmount}, Difficulty: {SelectedDifficulty}, Type: {SelectedType}");

            // Navigate to the game page with the selected settings
            await Shell.Current.GoToAsync($"GamePage?category={categoryId}&amount={SelectedAmount}&difficulty={SelectedDifficulty}&type={SelectedType}");
        }




    }
}
