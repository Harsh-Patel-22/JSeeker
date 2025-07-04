using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Backend.Services;

public class GithubService {
    private readonly HttpClient _httpClient;

    // TODO - Add a check if the passes repos belong to the owner. Verify with the database data about the same
    
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

    public async Task<Dictionary<string, JsonElement>> GetAllProjects() {
        var projects = new Dictionary<string, JsonElement>();
        // var owner = "Harsh-Patel-22"; // TODO - Replace the hard coded value to read from database
        var owner = "comfyanonymous"; 
        
        var repos = await GetUserReposAsync(owner);
        
        foreach (var repo in repos.EnumerateArray()) {
            var projectName = repo.GetProperty("name").ToString();
            var projectDetails = await GetAllInsightsFromRepoAsync(projectName);
            projects.Add(projectName, projectDetails);
        }
        
        return projects;
    }
    
    public async Task<JsonElement> GetAllInsightsFromRepoAsync(string repoName) {
        // var owner = "Harsh-Patel-22"; // TODO - replace the hard coded string with actual fetching from database 
        var owner = "comfyanonymous";
        var rawJson = await GetRepoAsync(owner, repoName);
        
        var htmlUrl = rawJson.GetProperty("html_url").ToString(); 
        var description = rawJson.GetProperty("description").ToString();
        var isPublic = rawJson.GetProperty("visibility").ToString().Equals("public");

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
            lanuages = languageUsages,
            readme = readmeContents,
            config = configContents
        }, new JsonSerializerOptions { WriteIndented = true });
        
        // TODO - Clean the configContents (remove the \n and other escape sequences and feed it to the ai to get the list of technologies and framework used. Use that reponse to then generate a killer resume description based on that.)
        // TODO - Send all the data to the ai service to get the content for description and other things like contents for projects section in the resume. 
        // TODO - For the profile section, give 2 options to add the projects, manual way and automated way with ai scraping the github
        rawJson = await GetAllLanguagesAsync(); // List of frameworks and languages
        return returnJson;
    }
    
    public async Task<JsonElement> GetRepoLanguagesAsync(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/languages");
        //Console.WriteLine($"{_httpClient.BaseAddress}repos/facebook/react/languages");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>();
        
    }
    public async Task<JsonElement> GetUserReposAsync(string owner) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}users/{owner}/repos");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }
    
    public async Task<JsonElement> GetRepoAsync(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }
    
    public async Task<string> GetRepoTopicsAsync(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/topics");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
    
    public async Task<string> GetRepoTagsAsync(string owner, string repo) {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}repos/{owner}/{repo}/tags");
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().Result;
    }
    
    public async Task<string> GetRepoReadmeAsync(string owner, string repo) {
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
}