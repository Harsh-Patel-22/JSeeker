using System.Text;
using System.Text.Json;

namespace Backend.Services;

public class ResumeBuilderService (AIService aiService, GithubService githubService) {
    private static readonly string BasePromptRegardingJson = "Take the following json. I want my output in the content-type: application/json format as well. Also, don't give me multiple options to select your responses from. Give me a single json back.";
    
    public async Task<JsonElement> GetResumeDescription() {
        var projectDetails = await GetProjectsDetailsAsync();
        var response = await aiService.GetChatResponseAsync($"{BasePromptRegardingJson}. Here's the json that I have regarding my projects: {projectDetails}. I want the output to have a resume description about me including the technologies and framework I used, regarding other things like some design patterns and architectures used. Give me all work related things from the projects json given above.");
        var cleanedResponse = GetCleanJsonString(response);
        return JsonSerializer.Deserialize<JsonElement>(cleanedResponse);
    }

    public async Task<JsonElement> GetProjectsDetailsAsync() {
        // TKey - project name, TValue - json element with all the necessary details.
        var projectDetails = await githubService.GetAllProjects();
        var response = await aiService.GetChatResponseAsync(
            $"{BasePromptRegardingJson} Give me a project description for the projects. Pick few that you think are the best and give me descriptions. It is for my resume so write accordingly to seek attention and rank higher. Consider this format for json: {{ project name: ..., project description: ..., and others so on.}}. Make sure to not miss out on any key details. {projectDetails} This is the details which I have. ");
        var cleanedResponse = GetCleanJsonString(response);
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(cleanedResponse);
        return jsonElement;
    }

    private string GetCleanJsonString(string json) {
        var cleanedString = json.Replace("```json", "").Replace("```", "").Replace("\n", "").Trim();
        int firstBrace = cleanedString.IndexOf('{');
        if (firstBrace >= 0)
            cleanedString = cleanedString.Substring(firstBrace);

        return cleanedString;
        
    }
}