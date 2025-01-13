using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.MAUI.Helpers
{
    public static class CsvLoader
    {
        public static List<ValidCombination> LoadValidCombinations()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "QuizApp.MAUI.Resources.Data.ValidCombinations.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream!);

            var validCombinations = new List<ValidCombination>();

            // Läs filen rad för rad
            foreach (var line in reader.ReadToEnd().Split('\n').Skip(1)) // Skip header
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                validCombinations.Add(new ValidCombination
                {
                    CategoryId = int.Parse(parts[0], CultureInfo.InvariantCulture),
                    CategoryName = parts[1],
                    Amount = int.Parse(parts[2], CultureInfo.InvariantCulture),
                    Difficulty = parts[3],
                    Type = parts[4]
                });
            }

            return validCombinations;
        }
    }

    // Modell för att representera kombinationer
    public class ValidCombination
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Amount { get; set; }
        public string Difficulty { get; set; }
        public string Type { get; set; }
    }
}
