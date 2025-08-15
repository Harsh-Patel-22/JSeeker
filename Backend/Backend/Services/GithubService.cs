using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Backend.Util;

namespace Backend.Services;

public class GithubService {
    private readonly HttpClient _httpClient;
    
    public GithubService(HttpClient httpClient) {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.github.com");
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Backend", "1.0"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("GITHUB_ACCESS_TOKEN"));
    }

    private async Task<Dictionary<string,string>> GetConfigFileContentsAsync(string owner, string repo) {
        var response = await _httpClient.GetAsync($"repos/{owner}/{repo}/contents/");
        response.EnsureSuccessStatusCode();

        var possibleConfigFilesList = new string[] {
            "package.json", "requirements.txt", "README.md", "pom.xml", ".csproj", "Dockerfile", "build.gradle", "composer.json"
        };
        
        var result = new Dictionary<string, string>();
        
        var root = await response.Content.ReadFromJsonAsync<JsonElement>();
        foreach (var configFile in root.EnumerateArray()) {
            var name =  configFile.GetProperty("name").GetString();
            var downloadUrl = configFile.GetProperty("download_url").GetString();

            if (possibleConfigFilesList.Contains(name, StringComparer.OrdinalIgnoreCase)) {
                var fileContents = _httpClient.GetStringAsync(downloadUrl).Result;
                result.Add(name, fileContents);                
            }
        }

        return result;
    }
    
    public async Task<JsonElement> GetAllInsightsFromRepoAsync(string owner, string repoName) {
        // owner = "comfyanonymous";
        var rawJson = await GetRepoAsync(owner, repoName);
        
        var htmlUrl = rawJson.GetProperty("html_url").ToString(); 
        var description = rawJson.GetProperty("description").ToString();
        var isPublic = rawJson.GetProperty("visibility").ToString().Equals("public");
        var createdAt = rawJson.GetProperty("created_at").ToString();
        var updatedAt = rawJson.GetProperty("updated_at").ToString();

        var repoLanguagesWithUsages = await GetRepoLanguagesAsync(owner, repoName);
        var languageUsages = new Dictionary<string, float>();
            
        var totalUsages = repoLanguagesWithUsages.EnumerateObject().Sum(property => property.Value.GetInt32());
        
        foreach (var property in repoLanguagesWithUsages.EnumerateObject()) {
            languageUsages.Add(property.Name, ((float) property.Value.GetInt32() / totalUsages) * 100);
        }

        string readmeContents = string.Empty;
        try {
            readmeContents = await GetRepoReadmeAsync(owner, repoName);
        }
        catch (HttpRequestException e) {
            if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                Console.WriteLine("Readme not found");
            }
        }
        var configContents = await GetConfigFileContentsAsync(owner, repoName); 
        var returnJson = JsonSerializer.SerializeToElement(new {
            
            isPublic = isPublic.ToString(),
            desc = description,
            url = htmlUrl,
            languages = languageUsages,
            readme = readmeContents,
            config = configContents,
            startedAt = createdAt,
            updatedAt = updatedAt,
        }, new JsonSerializerOptions { WriteIndented = true });
        
        // TODO - Clean the configContents (remove the \n and other escape sequences and feed it to the ai to get the list of technologies and framework used. Use that reponse to then generate a killer resume description based on that.)
        // TODO - Send all the data to the ai service to get the content for description and other things like contents for projects section in the resume. 
        
        // rawJson = await GetAllLanguagesAsync(); // List of frameworks and languages
        return returnJson;
    }
    
    private async Task<JsonElement> GetRepoLanguagesAsync(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/languages");
        //Console.WriteLine($"{_httpClient.BaseAddress}repos/facebook/react/languages");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>();
        
    }
    private async Task<List<JsonElement>> GetUserPublicReposAsync(string owner) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}users/{owner}/repos");
        response.EnsureSuccessStatusCode();
        var repos = await response.Content.ReadFromJsonAsync<JsonElement>();
        List<JsonElement> userPublicRepos = new List<JsonElement>();
        
        foreach (var repo in repos.EnumerateArray()) {
            if (repo.GetProperty("visibility").ToString().Equals("public")) {
                userPublicRepos.Add(repo);
            }
        }
        return userPublicRepos;
    }
    
    private async Task<JsonElement> GetRepoAsync(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}");
        // TODO - Maybe replace the EnsureSuccessStatusCode to a custom exception
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }
    
    private async Task<string> GetRepoReadmeAsync(string owner, string repo) {
        // TODO - Add error catch if no readme file exists in the repo
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/readme");
        response.EnsureSuccessStatusCode();
        var readmeString = response.Content.ReadAsStringAsync().Result;
        
        using var doc = JsonDocument.Parse(readmeString);
        var root = doc.RootElement;
    
        if (!root.TryGetProperty("content", out var contentElement))
            throw new Exception("Content field not found in GitHub response.");
    
        var base64Content = contentElement.GetString();
    
        if (string.IsNullOrWhiteSpace(base64Content))
            return string.Empty;
    
        // Remove any line breaks in base64 string (GitHub adds \n every 76 chars)
        base64Content = base64Content.Replace("\n", "").Replace("\r", "");
    
        var bytes = Convert.FromBase64String(base64Content);
        var decodedString = Encoding.UTF8.GetString(bytes);
        return decodedString;
    }
    
    public async Task<JsonElement> GetAllLanguagesAsync() {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}languages");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    public async Task<List<string>> GetAllRepoNamesAsync(string owner) {
        var repos = await GetUserPublicReposAsync(owner);
        // repos.GetString("");
        List<string> repoNames = new List<string>();
        foreach (var repo in repos) {
            repoNames.Add(repo.GetProperty("name").ToString());
        }
        return repoNames;
    }

    public async Task<List<string>?> GetAll3MostRecentRepoNamesAsync(string owner) {
        var repos = await GetUserPublicReposAsync(owner);
        List<string>? repoNames = new List<string>();
        
        var sortedRepos = repos.OrderByDescending(repo => repo.GetProperty("updated_at").GetDateTime()).ToList();

        for (int i = 0; i < 3; i++) {
            repoNames.Add(sortedRepos[i].GetProperty("name").ToString());
        }
        return repoNames;
    }
    
    
    // private async Task<string> GetRepoTopicsAsync(string owner, string repo) {
    //     var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/topics");
    //     response.EnsureSuccessStatusCode();
    //     return response.Content.ReadAsStringAsync().Result;
    // }
    //
    // private async Task<string> GetRepoTagsAsync(string owner, string repo) {
    //     var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/tags");
    //     response.EnsureSuccessStatusCode();
    //     return response.Content.ReadAsStringAsync().Result;
    // }

}