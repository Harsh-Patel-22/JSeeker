using System.Net.Http.Headers;

namespace Backend.Services;

public class GithubService {
    private readonly HttpClient _httpClient;

    public GithubService(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.github.com");
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Backend", "1.0"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("GITHUB_ACCESS_TOKEN"));
    }
    
    public async Task<string> GetRepoLanguages(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}");
        //Console.WriteLine($"{_httpClient.BaseAddress}repos/facebook/react/languages");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
    public async Task<string> GetUserRepos(string owner) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}users/{owner}/repos");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
    
    public async Task<string> GetRepo(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
    
    public async Task<string> GetRepoTopics(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/topics");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
    
    public async Task<string> GetRepoTags(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/tags");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
    
    public async Task<string> GetRepoReadme(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/readme");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
    
    public async Task<string> GetAllLanguages() {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}languages");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
}