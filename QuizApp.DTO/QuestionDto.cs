using Newtonsoft.Json;

namespace QuizApp.DTO
{
    public class QuestionDto
    {
        [JsonProperty("category")]
        public string Category { get; set; } = null!;

        [JsonProperty("type")]
        public string Type { get; set; } = null!;

        [JsonProperty("difficulty")]
        public string Difficulty { get; set; } = null!;

        [JsonProperty("question")]
        public string Question { get; set; } = null!;

        [JsonProperty("correct_answer")]
        public string CorrectAnswer { get; set; } = null!;

        [JsonProperty("incorrect_answers")]
        public List<string> IncorrectAnswers { get; set; } = new List<string>();
    }
}
