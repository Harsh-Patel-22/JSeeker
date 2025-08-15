using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Users;
using Backend.DTOs.Users.Hirer;
using Backend.Interfaces;
using Backend.Models.Mapping;
using Backend.Models.Users;
using Backend.Models.Users.Cocurricular;
using Backend.Models.Users.WorkRelated;
using Backend.Util;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository (ApplicationDbContext context, AddressRepository addressRepository) : IProjectHolder {
    
    // Section - Register User as Hirer
    public async Task RegisterUserAsHirerAsync(Guid userId, HirerProfessionalDetailsDto dto, int companyAddressId) {
        
        Hirer h = new Hirer {
            Id = userId,
            CompanyName = dto.CompanyName,
            Designation = dto.Designation,
            WebsiteLink = dto.WebsiteLink,
            CompanyAddressId = companyAddressId,
        };
        
        await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter => setter.SetProperty(user => user.IsHirer, true));
        await DbUpdateHelper.UpdateAllFieldsExceptAsync(h, context, "Id");
    }

    public async Task UpdateUserDetailsAsync(Guid userId, UserSecondaryDetailsDto dto) {
        int addressId = await addressRepository.CreateAddressAsync(dto.Address);
        
        User u = new User() {
            Id = userId,
            Gender = dto.Gender,
            AddressId = addressId,
            AboutLine = dto.AboutLine,
            Description = dto.Description,
            LinkedInProfileLink = dto.LinkedInProfileLink,
        };
        
        // TODO - Add a service/repo for the join classes. To enter the records for join entities along with base....
        foreach (var workExperience in dto.WorkExperienceDetails) {
            await context.WorkExperiences.AddAsync(new WorkExperience() {
                UserId = userId,
                Role = workExperience.Role,
                Description = workExperience.Description,
                CompanyName = workExperience.CompanyName,
                StartDate = workExperience.StartDate,
                EndDate = workExperience.EndDate
            });
        }

        foreach (var edto in dto.EducationDetails) {
            await context.Educations.AddAsync(new Education() {
                UserId = userId,
                Study = edto.Study,
                InstituteName = edto.InstituteName,
                Country = edto.Country,
                State = edto.State,
                StartDate = edto.StartDate,
                EndDate = edto.EndDate
            });
        }

        foreach (var langDto in dto.VocalLanguageDetails) {
            int languageId = await GetLanguageIdByNameAsync(langDto.Name);
            await context.UserVocalLanguages.AddAsync(new UserVocalLanguage() {
                UserId = userId,
                Level = langDto.Level,
                VocalLanguageId = languageId,
            });
        }
        // The below function executes and SaveChangesAsync() therefore no need to call it anywhere else here
        await DbUpdateHelper.UpdateAllFieldsExceptAsync(u, context, "Id", "PhoneNumber", "Email", "FirstName", "LastName", "GithubUsername", "NumberOfSuccessfulEmployments", "NumberOfRejections", "TechnicalKeywords");
        
        // TODO - Take the below code to else when the project details are added from github!
       }

    public async Task UpdateGithubUsernameFieldAsync(Guid userId, string githubUsername) {
        await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter => setter.SetProperty(user => user.GithubUsername, githubUsername));
    }

    public async Task<string> GetGithubUsernameAsync(Guid userId) {
        return (await context.Users.Where(user => user.Id == userId).Select(user => user.GithubUsername).FirstOrDefaultAsync())!;
    }
    
    public async Task<int> GetLanguageIdByNameAsync(string languageName) {
        VocalLanguage? language = await context.VocalLanguages.Where(language => language.Name == languageName).FirstOrDefaultAsync();
        if (language != null) {
            return language.Id;
        }

        VocalLanguage lan = new VocalLanguage() {
            Name = languageName
        };
        await context.VocalLanguages.AddAsync(lan);
        await context.SaveChangesAsync();
        return lan.Id;
    }
    // TODO - Add a check if project already added
    
    // Section - Resume/Keywords Related
    public async Task SetKeywordsAsync(Guid userId, List<string> keywords, string AIGeneratedKeywordsCSV) {
        var csvString = string.Join(",", keywords);

        await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter => setter.SetProperty(user => user.TechnicalKeywords, csvString).SetProperty(user => user.AIGeneratedTechnicalKeywords, AIGeneratedKeywordsCSV));
        
    }
    
    public async Task SetResumeJsonStringAsync(Guid userId, string ResumeJsonString) {
        await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter => setter.SetProperty(user => user.ResumeJsonString , ResumeJsonString));
    }

    public async Task SetResumeTemplateNumberAsync(Guid userId, int resumeTemplateNumber) {
        await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter => setter.SetProperty(user => user.ResumeTemplateNumber, resumeTemplateNumber));
    }
    
    public async Task<string?> GetResumeJsonStringAsync(Guid userId) {
        return await context.Users.Where(user => user.Id == userId).Select(user => user.ResumeJsonString).FirstOrDefaultAsync();
    }
    
    
    // Section - Fetching
    public async Task<UserProfessionalDetailsDto> GetUserProfessionalDetailsAsync(Guid userId) {
        // TODO - Remove the inidvidual methods....
        return new UserProfessionalDetailsDto(
            await GetProjectsAsync(userId),
            await GetWorkExperienceDetailsAsync(userId),
            await GetEducationDetailsAsync(userId)
            );
    }
    
    // TODO - Might wanna remove the dictionary since the name is already there in the detailsDto...
    public async Task<Dictionary<string, ProjectDetailsDto>> GetProjectsDetailsAsync(Guid userId) {
        Dictionary<string, ProjectDetailsDto> projectDetails = new Dictionary<string, ProjectDetailsDto>();
        List<Project> projects = await context.Projects.Where(project => project.UserId == userId).ToListAsync();
        foreach (var project in projects) {
            List<TechnologyUsageDto> usage = await context.ProjectTechnologies.Where(pt => pt.ProjectId == project.Id).Select(pt => new TechnologyUsageDto(pt.Technology.Name, pt.PercentUsage)).ToListAsync();
            projectDetails.Add(project.Name, new ProjectDetailsDto(project.Name, project.Description, usage, project.StartDate, project.LastUpdatedDate, project.GithubRepoLink));
        }
        return projectDetails;
    }
    private async Task<List<TechnologyUsageDto>> GetTechnologyUsagesAsync(int projectId) {
        var technologiesList = await context.ProjectTechnologies.Include(pt => pt.Technology).Where(pt => pt.ProjectId == projectId).Select(pt => new TechnologyUsageDto(pt.Technology.Name, pt.PercentUsage)).ToListAsync();
        return technologiesList;
    }
    
    // Section - Updating the metric fields
    public async Task IncrementSuccessCountAsync(Guid userId) {
        try {
            await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter =>
                setter.SetProperty(user => user.NumberOfSuccessfulEmployments,
                    user => user.NumberOfSuccessfulEmployments + 1));
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }

    public async Task IncrementRejectedCountAsync(Guid userId) {
        try {

            await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter =>
                setter.SetProperty(user => user.NumberOfSuccessfulEmployments,
                    user => user.NumberOfSuccessfulEmployments + 1));
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }
    
    // Section - Profile Related
    public async Task<BasicDetailsDto?> GetBasicDetailsAsync(Guid userId) {
        var details = await (from user in context.Users
            join address in context.Addresses on user.AddressId equals address.Id 
            where user.Id == userId
            select new BasicDetailsDto(
                user.FirstName, 
                user.LastName,  
                address.State, 
                address.Country, 
                user.AboutLine
            )).FirstOrDefaultAsync();
        
        return details;
    }


    public async Task<List<LanguageDto>> GetVocalLanguagesAsync(Guid userId) {
        var languages = await (from mapping in context.UserVocalLanguages
            join language in context.VocalLanguages on mapping.VocalLanguageId equals language.Id 
            where mapping.UserId == userId 
            select new LanguageDto(
                language.Name,
                mapping.Level
            )).ToListAsync();
        
        return languages;
    }

    public async Task<ContactDetailsDto?> GetContactDetailsAsync(Guid userId) {
        var contactDetails = await (from userCredential in context.UserCredentials
            join user in context.Users on userCredential.UserId equals user.Id
            where user.Id == userId
            select new ContactDetailsDto(
                userCredential.Email,
                user.GithubUsername,
                user.LinkedInProfileLink,
                user.PhoneNumber
            )).FirstOrDefaultAsync();
        
        return contactDetails;
    }
    
    // TODO - Make the bellow 3 functions private or just dump this code into a single function above created.... Just have the professional details dto for exchange...
    public async Task<List<ProjectDetailsDto>> GetProjectsAsync(Guid userId) {
        List<Project> userProjectModels = await context.Projects.Where(project => project.UserId == userId).ToListAsync();
        Dictionary<int,List<TechnologyUsageDto>> technologiesDictionary = new Dictionary<int, List<TechnologyUsageDto>>();
        
        foreach (var userProject in userProjectModels) {
            var technologiesList = await GetTechnologyUsagesAsync(userProject.Id);
            
            technologiesDictionary.Add(userProject.Id, technologiesList);
        }
        
        List<ProjectDetailsDto> projectDetails = new List<ProjectDetailsDto>();
        foreach (var userProject in userProjectModels) {
            projectDetails.Add(new ProjectDetailsDto(userProject.Name, userProject.Description, technologiesDictionary[userProject.Id], userProject.StartDate, userProject.LastUpdatedDate, userProject.GithubRepoLink));
        }
        
        return projectDetails;
    }

    public async Task<List<EducationDetailsDto>> GetEducationDetailsAsync(Guid userId) {
        var educationDetails = await (from education in context.Educations where education.UserId == userId select new EducationDetailsDto(education.Study, education.InstituteName, education.State, education.Country, education.StartDate, education.EndDate)).ToListAsync();  
        return educationDetails;
    }

    public async Task<List<WorkExperienceDetailsDto>> GetWorkExperienceDetailsAsync(Guid userId) {
        var workExperienceDetails = await (from workExperience in context.WorkExperiences where workExperience.UserId == userId select new WorkExperienceDetailsDto(workExperience.Role, workExperience.Description, workExperience.CompanyName, workExperience.StartDate, workExperience.EndDate)).ToListAsync();  
        return workExperienceDetails;
    }
}