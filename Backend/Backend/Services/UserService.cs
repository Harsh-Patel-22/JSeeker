using System.Text.Json;
using Backend.DTOs;
using Backend.Exceptions;
using Backend.Models.Users.WorkRelated;
using Backend.Repositories;
using Backend.Util;

namespace Backend.Services;

public class UserService (GithubService githubService, UserRepository userRepository) {
    public async Task<List<string>> GetAllProjectNamesAsync(string githubUsername) {
        List<string> repoNames = await githubService.GetAllRepoNamesAsync(githubUsername);
        return repoNames;
    }
    
    public async Task UpdateProjectsUsingGithubUsernameAsync(Guid userId, string githubUsername, List<string> repoNames) {
        List<ProjectTechnologyMappingDto> projectTechnologyMappings = new List<ProjectTechnologyMappingDto>();
        if (repoNames.Count > 3) {
            throw new GlobalExceptions.ProjectLimitExceeded();
        }
        
        foreach (var repoName in repoNames) {
            var allInsights = await githubService.GetAllInsightsFromRepoAsync(githubUsername, repoName);

            if (allInsights.GetProperty("isPublic").ToString().Equals("private")) {
                throw new Exception("The given repository/project is private!!");
            }
            
            if (!DateOnly.TryParse(StringCleaner.GetCleanDateString(allInsights.GetProperty("startedAt").ToString()), out DateOnly createdAt)) {
                throw new GlobalExceptions.InvalidDate();
            }
            if (!DateOnly.TryParse(StringCleaner.GetCleanDateString(allInsights.GetProperty("updatedAt").ToString()), out DateOnly updatedAt)) {
                throw new GlobalExceptions.InvalidDate();
            }
            
            Project p = new Project() {
                Name = repoName,
                Description = allInsights.GetProperty("desc").ToString(),
                UserId = userId,
                StartDate = createdAt,
                LastUpdatedDate = updatedAt,
                GithubRepoLink = allInsights.GetProperty("url").ToString(),
            };
            
            var usageDictionary = allInsights.GetProperty("languages").EnumerateObject().ToDictionary(l => l.Name, l => float.Parse(l.Value.ToString()));
            foreach (var usage in usageDictionary) {
                Console.WriteLine(usage.Key + ": " + usage.Value);
            }
            Console.WriteLine(usageDictionary);
            await userRepository.AddGithubProjectAndMappingsAsync(new ProjectTechnologyMappingDto(
                p,
                usageDictionary
                ));
        }
    }
}