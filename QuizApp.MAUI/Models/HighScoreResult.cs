using SQLite;


namespace QuizApp.MAUI.Models
{
    public class HighScoreResult
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime Date { get; set; }
    }
}
