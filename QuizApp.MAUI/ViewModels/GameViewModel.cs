using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizApp.Core;
using QuizApp.MAUI.Models;
using System.Collections.ObjectModel;
using System.Web;


namespace QuizApp.MAUI.ViewModels
{
    [ObservableObject]
    [QueryProperty(nameof(Category), "category")]
    [QueryProperty(nameof(Amount), "amount")]
    [QueryProperty(nameof(Difficulty), "difficulty")]
    [QueryProperty(nameof(Type), "type")]
    public partial class GameViewModel
    {
        private readonly TriviaApiService _triviaApiService;

        [ObservableProperty]
        private List<Question> questions = new();

        [ObservableProperty]
        private Question currentQuestion = new();

        [ObservableProperty]
        private ObservableCollection<string> currentQuestionAnswers = new();

        [ObservableProperty]
        private string selectedAnswer = string.Empty;

        [ObservableProperty]
        private bool isAnswerSelected;

        [ObservableProperty]
        private bool isAnswerSubmitted;

        [ObservableProperty]
        private int currentQuestionIndex;

        [ObservableProperty]
        private int score;

        // Query Properties with custom logic in set
        private string _category = string.Empty;
        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                Console.WriteLine($"Category set to: {_category}");
                TryLoadQuestions();
            }
        }

        private int _amount;
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                Console.WriteLine($"Amount set to: {_amount}");
                TryLoadQuestions();
            }
        }

        private string _difficulty = string.Empty;
        public string Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                Console.WriteLine($"Difficulty set to: {_difficulty}");
                TryLoadQuestions();
            }
        }

        private string _type = string.Empty;
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                Console.WriteLine($"Type set to: {_type}");
                TryLoadQuestions();
            }
        }

        public GameViewModel(TriviaApiService triviaApiService)
        {
            _triviaApiService = triviaApiService;
        }


        // Method to try loading questions once all parameters are set
        private void TryLoadQuestions()
        {
            if (!string.IsNullOrEmpty(Category) && Amount > 0 && !string.IsNullOrEmpty(Difficulty) && !string.IsNullOrEmpty(Type))
            {
                LoadQuestionsCommand.Execute(null);
            }
        }

        [RelayCommand]
        private async Task LoadQuestions()
        {
            try
            {
                Console.WriteLine($"Loading questions with Category: {Category}, Amount: {Amount}, Difficulty: {Difficulty}, Type: {Type}");
                // Get DTOs from the service
                var questionDtos = await _triviaApiService.GetQuestionsAsync(Amount, Category, Difficulty, Type);

                // Map DTOs to models
                Questions = questionDtos.Select(dto => new Question
                {
                    Category = dto.Category,
                    Type = dto.Type,
                    Difficulty = dto.Difficulty,
                    QuestionText = HttpUtility.UrlDecode(dto.Question),
                    CorrectAnswer = HttpUtility.UrlDecode(dto.CorrectAnswer),
                    IncorrectAnswers = dto.IncorrectAnswers.Select(answer => HttpUtility.UrlDecode(answer) ?? string.Empty).ToList()
                }).ToList();

                if (Questions.Any())
                {
                    CurrentQuestionIndex = 0;
                    LoadCurrentQuestion();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

           
        }

        private void LoadCurrentQuestion()
        {
            CurrentQuestion = Questions[CurrentQuestionIndex];
            var answers = new List<string>(CurrentQuestion.IncorrectAnswers) { CurrentQuestion.CorrectAnswer };
            CurrentQuestionAnswers = new ObservableCollection<string>(answers.OrderBy(_ => Guid.NewGuid()));
            SelectedAnswer = string.Empty;
            IsAnswerSelected = false;
            IsAnswerSubmitted = false;
        }

        [RelayCommand]
        private void SubmitAnswer()
        {
            if (SelectedAnswer == CurrentQuestion.CorrectAnswer)
            {
                Score++;
            }

            IsAnswerSubmitted = true;
        }

        [RelayCommand]
        private void NextQuestion()
        {
            if (CurrentQuestionIndex + 1 < Questions.Count)
            {
                CurrentQuestionIndex++;
                LoadCurrentQuestion();
            }
            else
            {
                // Navigate to ResultsPage
                Shell.Current.GoToAsync($"ResultsPage?score={Score}&total={Questions.Count}");
            }
        }

        // Receive navigation parameters
        public void ApplyNavigationParameters(IDictionary<string, object> parameters)
        {
            if (parameters.TryGetValue("Category", out var category))
                Category = category?.ToString() ?? string.Empty;

            if (parameters.TryGetValue("Amount", out var amount))
                Amount = Convert.ToInt32(amount);

            if (parameters.TryGetValue("Difficulty", out var difficulty))
                Difficulty = difficulty?.ToString() ?? string.Empty;

            if (parameters.TryGetValue("Type", out var type))
                Type = type?.ToString() ?? string.Empty;

            Console.WriteLine($"Loaded parameters - Category: {Category}, Amount: {Amount}, Difficulty: {Difficulty}, Type: {Type}");
        }
    }
}
