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
            // Försöksräknare
            int retries = 3;

            while (retries > 0)
            {
                try
                {
                    // See if we have a session sessionToken
                    if (string.IsNullOrEmpty(_sessionToken))
                    {
                        _sessionToken = await GetSessionTokenAsync();
                    }

                    // Base-URL for API
                    string baseUrl = "https://opentdb.com/api.php";

                    // Query parameters
                    var queryParameters = new List<string>
                    {
                        $"amount={amount}",
                        $"sessionToken={_sessionToken}",
                        $"encode={encoding}" // Default is URL encoding
                    };

                    if (!string.IsNullOrEmpty(category))
                        queryParameters.Add($"category={category}");
                    if (!string.IsNullOrEmpty(difficulty))
                        queryParameters.Add($"difficulty={difficulty}");
                    if (!string.IsNullOrEmpty(type))
                        queryParameters.Add($"type={type}");

                    // Construct the full URL
                    string url = $"{baseUrl}?{string.Join("&", queryParameters)}";

                    // Fetch the data
                    var response = await _httpClient.GetStringAsync(url);

                    // Deserialize JSON-response
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);


                    //Controll response code
                    if (apiResponse == null)
                    {
                        throw new Exception("Failed to parse API response.");
                    }

                    if (apiResponse.ResponseCode == 1) // No Results
                    {
                        return new List<QuestionDto>();
                    }
                    else if (apiResponse.ResponseCode == 4) // Token expired
                    {
                        _sessionToken = await GetSessionTokenAsync();
                        continue;
                    } else if (apiResponse.ResponseCode != 0) // Other errors
                    {
                        throw new Exception($"Failed to retrieve questions: Response Code {apiResponse.ResponseCode}");
                    }

                    return apiResponse.Results ?? new List<QuestionDto>();
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    Console.WriteLine("Rate limit hit, waiting...");
                    await Task.Delay(5000); // Wait 5 seconds
                    retries--;
                }
            }

            throw new Exception("Too many requests, even after retries.");
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

        private async Task<string> RetryOnRateLimitAsync(Func<Task<string>> apiCall, int maxRetries = 3, int delayMilliseconds = 2000)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    return await apiCall();
                }
                catch (Exception ex) when (ex.Message.Contains("Too many requests"))
                {
                    retries++;
                    await Task.Delay(delayMilliseconds);
                }
            }
            throw new Exception("Too many requests, even after retries.");
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
