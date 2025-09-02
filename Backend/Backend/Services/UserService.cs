using System.Text.Json;
using Backend.DTOs;
using Backend.DTOs.Users;
using Backend.Exceptions;
using Backend.Models.Users.WorkRelated;
using Backend.Repositories;
using Backend.Util;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Backend.Services;

public class UserService (GithubService githubService, UserRepository userRepository, ProjectsRepository projectsRepository, ValidationService validationService, ResumeBuilderService resumeBuilder, PdfService pdfService) {
    public async Task<List<string>> GetAllProjectNamesAsync(Guid userId) {
        List<string> repoNames = await githubService.GetAllRepoNamesAsync(await userRepository.GetGithubUsernameAsync(userId));
        return repoNames;
    }

    // public async Task<ResumeContentsDto> GetResumeContentsAsync(Guid userId) {
    //     return new ResumeContentsDto() {
    //         BasicDetails = await userRepository.GetBasicDetailsAsync(userId),
    //         ContactDetails = await userRepository.GetContactDetailsAsync(userId),
    //         EducationDetails = await userRepository.GetEducationDetailsAsync(userId),
    //         LanguageDetails = await userRepository.GetVocalLanguagesAsync(userId),
    //         ProjectDetails = await userRepository.GetProjectsDetailsAsync(userId),
    //         WorkExperienceDetails = await userRepository.GetWorkExperienceDetailsAsync(userId)
    //     };
    // }

    public async Task UpdateUserDetailsAsync(Guid userId, UserSecondaryDetailsDto dto) {
        await userRepository.UpdateUserDetailsAsync(userId, dto);
    }

    public async Task UpdateGithubUsernameAsync(Guid userId,  string githubUsername) {
        await userRepository.UpdateGithubUsernameFieldAsync(userId, githubUsername);
    }
    
    public async Task UpdateProjectsUsingGithubReposAsync(Guid userId, List<string>? repoNames) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new GlobalExceptions.Unauthorised();
        }

        List<JsonElement> allInsightsList;
        
        if(repoNames == null) {
            repoNames = await githubService.GetTop3RepoNamesAsync(await userRepository.GetGithubUsernameAsync(userId));
        }
        
        if (repoNames.Count > 3) {
            throw new GlobalExceptions.ProjectLimitExceeded();
        }
        
        
        Dictionary<string, ProjectDetailsDto> projectDetails = new Dictionary<string, ProjectDetailsDto>();
        List<string> keywords = new List<string>();
        
        // An Array could be used rather than a List<JsonElement>
        allInsightsList = new List<JsonElement>(repoNames.Count);
        
        foreach (var repoName in repoNames) {
            var allInsights = await githubService.GetAllInsightsFromRepoAsync(await userRepository.GetGithubUsernameAsync(userId), repoName);
            allInsightsList.Add(allInsights);
            
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
            
            var usage = allInsights.GetProperty("languages").EnumerateObject().Select(prop => new TechnologyUsageDto(prop.Name, prop.Value.GetSingle())).ToList();
            foreach (var usageDto in usage) {
                // Appending distinct technology names.
                if (!keywords.Contains(usageDto.Name)) {
                    keywords.Add(usageDto.Name);
                }
            }
            await projectsRepository.AddGithubProjectAndMappingsAsync(new ProjectTechnologyMappingDto(
                p,
                usage
                ));
            
            projectDetails.Add(repoName, new ProjectDetailsDto(p.Name, p.Description, usage, p.StartDate, p.LastUpdatedDate, p.GithubRepoLink));

        }

        string AIGenKeywords = await resumeBuilder.GetAIGeneratedKeywordsAsync(allInsightsList);
        
        string resumeString = await resumeBuilder.GetGeneratedResumeAsync(new ResumeContentsDto() {
            ProjectDetails = projectDetails,
            BasicDetails = await userRepository.GetBasicDetailsAsync(userId),
            ContactDetails = await userRepository.GetContactDetailsAsync(userId),
            WorkExperienceDetails = await userRepository.GetWorkExperienceDetailsAsync(userId),
            EducationDetails = await userRepository.GetEducationDetailsAsync(userId),
            LanguageDetails = await userRepository.GetVocalLanguagesAsync(userId)
        });

        await userRepository.SetKeywordsAsync(userId, keywords, AIGenKeywords);
        await userRepository.SetResumeJsonStringAsync(userId, resumeString);
        
    }

    public async Task UpdateResumeAsync(Guid userId, ResumeContentsDto resumeDto) {
        // TODO - Limit the adding/remove projects. Could only update the existing ones... Frontend handling.
        string resumeString = await resumeBuilder.GetGeneratedResumeAsync(resumeDto);

        await userRepository.SetResumeJsonStringAsync(userId ,resumeString);
    }
    
    public async Task SetResumeJsonStringAsync(Guid userId, string resumeJsonString) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new GlobalExceptions.Unauthorised();
        }
        await userRepository.SetResumeJsonStringAsync(userId, resumeJsonString);
    }

    public async Task SetResumeTemplateNumberAsync(Guid userId, int resumeTemplateNumber) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new GlobalExceptions.Unauthorised();
        }
        await userRepository.SetResumeTemplateNumberAsync(userId, resumeTemplateNumber);
    }
    
    public async Task<string> GetResumeJsonStringAsync(Guid userId) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new GlobalExceptions.Unauthorised();
        }
        return await userRepository.GetResumeJsonStringAsync(userId);
    }

    public async Task<Byte[]> GetResumePdfAsync(Guid userId) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new GlobalExceptions.Unauthorised();
        }

        ResumeTemplateAndStringDto resumeDetails = await userRepository.GetResumeAndTemplateNumberAsync(userId);
        if (resumeDetails.ResumeString == null || resumeDetails.TemplateNumber == null) {
            throw new GlobalExceptions.Unauthorised();
        }
        return await pdfService.GeneratePdfAsync(resumeDetails.ResumeString, resumeDetails.TemplateNumber);
    }

    public async Task<ResumeTemplateAndStringDto> GetResumeTemplateAndStringAsync(Guid userId) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new GlobalExceptions.Unauthorised();
        }

        return await userRepository.GetResumeAndTemplateNumberAsync(userId);
    }
}