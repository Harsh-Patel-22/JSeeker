using System.Net.Http.Headers;
using System.Text.Json;

namespace Backend.Services;

public class AIService {
    private readonly HttpClient _httpClient;
    private readonly GithubService _githubService;
    private readonly string _chatEndpoint;

    public AIService(HttpClient httpClient, GithubService githubService) {
        _httpClient = httpClient;
        _githubService = githubService;
        
        _httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com");
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Backend", "1.0"));
        
        string? apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        _chatEndpoint = $"v1beta/models/gemini-2.0-flash-lite:generateContent?key={apiKey}";
        // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _ApiKey);
    }

    public async Task<string> GetChatResponseAsync(string query) {
        var requestBody = new {
            contents = new[] {
                new {
                    parts = new [] {
                        new {
                            text = query
                        }
                    }
                }
        }
        };
        var response = await _httpClient.PostAsJsonAsync(_chatEndpoint, requestBody);
        response.EnsureSuccessStatusCode();
        var rawResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
        var stringResponse = rawResponse.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").ToString();
        return stringResponse;
    }

    // public async Task<JsonElement> GetResumeDescription() {
    //     
    // }
}