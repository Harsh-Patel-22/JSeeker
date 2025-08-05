using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Users;
using Backend.DTOs.Users.Hirer;
using Backend.Interfaces;
using Backend.Models.Mapping;
using Backend.Models.Users;
using Backend.Models.Users.WorkRelated;
using Backend.Util;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository (ApplicationDbContext context) : IProjectHolder {
    
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

    public async Task UpdateUserDetailsAsync(Guid userId) {
        
        // TODO - Add all details. The function could be really simple once I have the dto for it. I can directly fetch the things from it and maybe just 1 update call would be needed.
        User u = new User() {
            Id = userId,
            // Keywords = csvString,
        };
        
        await DbUpdateHelper.UpdateAllFieldsExceptAsync(u, context, "Id");
        
        List<string> keywords = await context.Projects
            .Where(p => p.UserId == userId)
            .SelectMany(p => p.ProjectTechnologies.Select(pt => pt.Technology.Name))
            .Distinct()
            .ToListAsync();
        string csvString = String.Join(",", keywords);
        
        await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter => setter.SetProperty(user => user.Keywords, csvString));
        
    }

    public async Task AddGithubProjectAndMappingsAsync(ProjectTechnologyMappingDto mapping) {
        await context.Projects.AddAsync(mapping.Project);
        await context.SaveChangesAsync();

        List<ProjectTechnology> projectTechnologies = new List<ProjectTechnology>();
        foreach (var technologyNameUsage in mapping.MappingData) {
            int techId = await GetTechnologyIdAsync(technologyNameUsage.Key);
            
            projectTechnologies.Add(new ProjectTechnology() {
                ProjectId = mapping.Project.Id,
                TechnologyId = techId,
                PercentUsage = technologyNameUsage.Value,
            });
        }
        
        await context.ProjectTechnologies.AddRangeAsync(projectTechnologies);
        await context.SaveChangesAsync();
    }

    private async Task<int> GetTechnologyIdAsync(string technologyName) {
        var tech = await context.Technologies.Where(technology => technology.Name.Equals(technologyName))
            .FirstOrDefaultAsync();

        if (tech != null) return tech.Id;
        Technology t = new Technology() {
            Name = technologyName
        };
        await context.Technologies.AddAsync(t);
        await context.SaveChangesAsync();
        return t.Id;
    }
    
    // Section - Resume Related
    public async Task SetResumeJsonStringAsync(Guid userId, string ResumeJsonString) {
        await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter => setter.SetProperty(user => user.ResumeJsonString , ResumeJsonString));
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
                user.Occupation, 
                address.State, 
                address.Country, 
                user.AboutLine
            )).FirstOrDefaultAsync();
        
        return details;
    }

    public async Task<List<HobbyDto>> GetHobbiesAsync(Guid userId) {
        var hobbies = await (from mapping in context.UserHobbies
            join hobby in context.Hobbies on mapping.HobbyId equals hobby.Id 
            where mapping.UserId == userId 
            select new HobbyDto(
                hobby.Name
            )).ToListAsync();
        
        return hobbies;
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

    public async Task<List<ProjectDetailsDto>> GetProjectsAsync(Guid userId) {
        List<Project> userProjectModels = await context.Projects.Where(project => project.UserId == userId).ToListAsync();
        Dictionary<int,List<TechnologyUsageDto>> technologiesDictionary = new Dictionary<int, List<TechnologyUsageDto>>();
        
        foreach (var userProject in userProjectModels) {
            var technologiesList = await (from projectTechnology in context.ProjectTechnologies
                join technology in context.Technologies on projectTechnology.TechnologyId equals technology.Id
                where projectTechnology.ProjectId == userProject.Id 
                select new TechnologyUsageDto(technology.Name, projectTechnology.PercentUsage)).ToListAsync();
            
            technologiesDictionary.Add(userProject.Id, technologiesList);
        }
        
        List<ProjectDetailsDto> projectDetails = new List<ProjectDetailsDto>();
        foreach (var userProject in userProjectModels) {
            projectDetails.Add(new ProjectDetailsDto(userProject.Name, technologiesDictionary[userProject.Id], userProject.StartDate, userProject.LastUpdatedDate, userProject.GithubRepoLink));
        }
        
        return projectDetails;
    }

    public async Task<List<EducationDetailsDto>> GetEducationDetailsAsync(Guid userId) {
        var educationDetails = await (from education in context.Educations where education.UserId == userId select new EducationDetailsDto(education.Study, education.InstituteName, education.State, education.Country, education.StartDate, education.EndDate)).ToListAsync();  
        return educationDetails;
    }

    public async Task<List<WorkExperienceDetailsDto>> GetWorkExperienceDetailsAsync(Guid userId) {
        var workExperienceDetails = await (from workExperience in context.WorkExperiences where workExperience.UserId == userId select new WorkExperienceDetailsDto(workExperience.Role, workExperience.Description, workExperience.State, workExperience.Country, workExperience.StartDate, workExperience.EndDate)).ToListAsync();  
        return workExperienceDetails;
    }
}