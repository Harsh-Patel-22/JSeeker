using System.Net.Http.Headers;
using System.Text.Json;

namespace Backend.Services;

public class AIService {
    private readonly HttpClient _httpClient;
    private readonly string _chatEndpoint;

    public AIService(HttpClient httpClient) {
        _httpClient = httpClient;
        
        _httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com");
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Backend", "1.0"));
        
        string? apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        _chatEndpoint = $"v1beta/models/gemini-2.5-flash-lite:generateContent?key={apiKey}";
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

    public JsonElement GetCleanedJson(string rawJsonString)
    {
        int start = rawJsonString.IndexOf('{');
        if (start == -1)
            throw new InvalidOperationException("No JSON object found.");

        int depth = 0;
        for (int i = start; i < rawJsonString.Length; i++)
        {
            if (rawJsonString[i] == '{') depth++;
            else if (rawJsonString[i] == '}') depth--;

            if (depth == 0)
            {
                string json = rawJsonString.Substring(start, i - start + 1);
                return JsonSerializer.Deserialize<JsonElement>(json);
            }
        }

        throw new InvalidOperationException("Incomplete JSON object.");
    }

}