using QuizApp.Core;
using System.Web;


namespace TriviaApiServiceTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Instantiate the Trivia API Service
            TriviaApiService triviaApiService = new TriviaApiService();

            // Test fetching categories
            Console.WriteLine("Fetching categories...");
            var categories = await triviaApiService.GetCategoriesAsync();
            Console.WriteLine("Categories:");
            foreach (var category in categories)
            {
                Console.WriteLine($"ID: {category.Id}, Name: {category.Name}");
            }

            // Test fetching questions
            Console.WriteLine("\nFetching questions...");
            var questions = await triviaApiService.GetQuestionsAsync(
                amount: 10,
                category: "9", // General Knowledge
                difficulty: "medium", // Medium difficulty
                type: "multiple", // Multiple Choice
                encoding: "url3986" // URL encoding for special characters
            );

            foreach (var question in questions)
            {
                Console.WriteLine($"Category: {question.Category}");
                Console.WriteLine($"Type: {question.Type}");
                Console.WriteLine($"Difficulty: {question.Difficulty}");
                Console.WriteLine($"Question: {HttpUtility.UrlDecode(question.Question)}");
                Console.WriteLine($"Correct Answer: {HttpUtility.UrlDecode(question.CorrectAnswer)}");
                Console.WriteLine("Incorrect Answers:");
                foreach (var answer in question.IncorrectAnswers)
                {
                    Console.WriteLine($" - {HttpUtility.UrlDecode(answer)}");
                }
                Console.WriteLine();
            }


            // Test session token reset
            Console.WriteLine("\nResetting session token...");
            await triviaApiService.ResetSessionTokenAsync();
            Console.WriteLine("Session token reset successfully.");

            Console.WriteLine("\nAll tests completed!");
        }
    }
}
