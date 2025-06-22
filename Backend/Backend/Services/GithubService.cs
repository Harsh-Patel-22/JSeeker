using System.Net.Http.Headers;

namespace Backend.Services;

public class GithubService {
    private readonly HttpClient _httpClient;

    public GithubService(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.github.com");
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Backend", "1.0"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("GithubToken"));
    }
    
    public async Task<string> GetRepoAsync(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/repos/{owner}/{repo}");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
}