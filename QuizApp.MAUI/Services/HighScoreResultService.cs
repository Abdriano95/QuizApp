using QuizApp.MAUI.Models;
using SQLite;


namespace QuizApp.MAUI.Services
{
    public class HighScoreResultService
    {
        private readonly SQLiteAsyncConnection _database;

        public HighScoreResultService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<HighScoreResult>().Wait();
        }

        public Task<List<HighScoreResult>> GetResultsAsync()
        {
            return _database.Table<HighScoreResult>().OrderByDescending(r => r.Score).ToListAsync();
        }

        public Task<int> SaveResultAsync(HighScoreResult result)
        {
            return _database.InsertAsync(result);
        }

        public Task<int> DeleteResultAsync(HighScoreResult result)
        {
            return _database.DeleteAsync(result);
        }

        // Reset the database
        public Task<int> ResetResultsAsync()
        {
            return _database.DeleteAllAsync<HighScoreResult>();
        }
    }
}
