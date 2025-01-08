using Newtonsoft.Json;
using QuizApp.DTO;


namespace QuizApp.Core
{
    public class TriviaApiService
    {
        private readonly HttpClient _httpClient;
        private string? _sessionToken;

        public TriviaApiService()
        {
            _httpClient = new HttpClient();
            _sessionToken = null;
        }

        // Get questions from Open Trivia DB
        public async Task<List<QuestionDto>> GetQuestionsAsync(int amount = 10, string? category = null, string? difficulty = null, string? type = null, string encoding = "url3986")
        {
            // Kontrollera om session sessionToken behövs
            if (string.IsNullOrEmpty(_sessionToken))
            {
                _sessionToken = await GetSessionTokenAsync();
            }

            // Bas-URL for API
            string baseUrl = "https://opentdb.com/api.php";

            // Build query parameters
            var queryParameters = new List<string>
                    {
                        $"amount={amount}",
                        $"sessionToken={_sessionToken}",
                        $"encode={encoding}" // Default: url3986 (handles special characters)
                    };

            if (!string.IsNullOrEmpty(category))
                queryParameters.Add($"category={category}");

            if (!string.IsNullOrEmpty(difficulty))
                queryParameters.Add($"difficulty={difficulty}");

            if (!string.IsNullOrEmpty(type))
                queryParameters.Add($"type={type}");

            // Combine URL and query parameters
            string url = $"{baseUrl}?{string.Join("&", queryParameters)}";

            // Do API-anrop
            var response = await _httpClient.GetStringAsync(url);

            // Deserialize JSON-response
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);

            // Controll response code
            if (apiResponse?.ResponseCode == 4) // Token expired
            {
                _sessionToken = await GetSessionTokenAsync();
                return await GetQuestionsAsync(amount, category, difficulty, type, encoding);
            }
            else if (apiResponse?.ResponseCode != 0) // Other errors
            {
                throw new Exception($"API Error: Response Code {apiResponse?.ResponseCode}");
            }
            else if (apiResponse.ResponseCode != 0) // Other errors
            {
                throw new Exception($"API Error: Response Code {apiResponse.ResponseCode}");
            }

            return apiResponse.Results ?? new List<QuestionDto>();
        }

        // Get session sessionToken
        public async Task<string> GetSessionTokenAsync()
        {
            string url = "https://opentdb.com/api_token.php?command=request";
            var response = await _httpClient.GetStringAsync(url);

            // Log the raw API response
            Console.WriteLine($"Raw API Response: {response}");

            var sessionTokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response);

            if (sessionTokenResponse == null)
            {
                throw new Exception("Session token response is null");
            }

            if (sessionTokenResponse.ResponseCode != 0)
            {
                throw new Exception($"Failed to retrieve session token: Response Code {sessionTokenResponse.ResponseCode}");
            }

            if (string.IsNullOrEmpty(sessionTokenResponse.Token))
            {
                throw new Exception("Session token is null or empty");
            }

            return sessionTokenResponse.Token;
        }




        // Get categories
        public async Task<List<Category>> GetCategoriesAsync()
        {
            string url = "https://opentdb.com/api_category.php";
            var response = await _httpClient.GetStringAsync(url);
            var categoryResponse = JsonConvert.DeserializeObject<CategoryResponse>(response);

            return categoryResponse?.TriviaCategories ?? new List<Category>();
        }

        // Reset session sessionToken
        public async Task ResetSessionTokenAsync()
        {
            Console.WriteLine($"Resetting session token: {_sessionToken}");

            if (string.IsNullOrEmpty(_sessionToken)) return;

            // Correct parameter name for token
            string url = $"https://opentdb.com/api_token.php?command=reset&token={_sessionToken}";
            var response = await _httpClient.GetStringAsync(url);
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response);


            if (tokenResponse == null || tokenResponse.ResponseCode != 0)
            {
                throw new Exception($"Failed to reset session token: Response Code {tokenResponse?.ResponseCode}");
            }
        }


        // Classes for JSON deserialization
        public class ApiResponse
        {
            [JsonProperty("response_code")]
            public int ResponseCode { get; set; }

            [JsonProperty("results")]
            public List<QuestionDto>? Results { get; set; }
        }

        public class TokenResponse
        {
            [JsonProperty("response_code")]
            public int ResponseCode { get; set; }

            [JsonProperty("token")]
            public string? Token { get; set; }
        }


        public class CategoryResponse
        {
            [JsonProperty("trivia_categories")]
            public List<Category>? TriviaCategories { get; set; }
        }

        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}
