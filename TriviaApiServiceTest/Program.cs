using QuizApp.Core;
using System.Web;
using System.IO;
using System.Threading;


namespace TriviaApiServiceTest
{
    public class Program
    {
        private const int DelayBetweenRequests = 1000; // 1 sec delay
        static async Task Main(string[] args)
        {
            // Instantiate the Trivia API Service
            TriviaApiService triviaApiService = new TriviaApiService();

            // Fetch categories
            Console.WriteLine("Fetching categories...");
            var categories = await triviaApiService.GetCategoriesAsync();
            Console.WriteLine("Categories fetched!");

            // Define difficulty levels and question types
            var difficulties = new[] { "easy", "medium", "hard" };
            var types = new[] { "multiple", "boolean" };
            var questionAmounts = new[] { 5, 10, 20 };



            // Store valid combinations
            var validCombinations = new List<string>();

            Console.WriteLine("Testing all combinations...");
            foreach (var category in categories)
            {
                foreach (var difficulty in difficulties)
                {
                    foreach (var type in types)
                    {
                        foreach (var amount in questionAmounts)
                        {
                            try
                            {
                                Console.WriteLine($"Testing: Category={category.Name}, Amount={amount}, Difficulty={difficulty}, Type={type}");

                                // Test the combination
                                var questions = await triviaApiService.GetQuestionsAsync(
                                    amount: amount,
                                    category: category.Id.ToString(),
                                    difficulty: difficulty,
                                    type: type
                                );

                                if (questions.Count > 0)
                                {
                                    Console.WriteLine("Valid combination!");
                                    validCombinations.Add($"{category.Id},{category.Name},{amount},{difficulty},{type}");
                                }
                                else
                                {
                                    Console.WriteLine("No questions available.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error testing combination: {ex.Message}");
                            }

                            // Wait before the next request to avoid hitting the rate limit
                            Console.WriteLine("Waiting before the next request...");
                            await Task.Delay(DelayBetweenRequests);
                        }
                    }
                }
            }

            Console.WriteLine("\nSaving valid combinations to file...");
            SaveValidCombinations(validCombinations);
            Console.WriteLine("Valid combinations saved!");
        }

        private static void SaveValidCombinations(List<string> validCombinations)
        {
            string filePath = "ValidCombinations.csv";
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("CategoryId,CategoryName,Amount,Difficulty,Type"); // Header
                foreach (var combination in validCombinations)
                {
                    writer.WriteLine(combination);
                }
            }
            Console.WriteLine($"Combinations saved to {filePath}");
        }
    }
}
