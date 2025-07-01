using Backend.Data;
using Backend.DTOs.Users;
using Backend.Models.Users.WorkRelated;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository (ApplicationDbContext context) {
    
    public async Task<BasicDetailsDto?> GetBasicDetailsAsync(int userId) {
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

    public async Task<List<HobbyDto>> GetHobbiesAsync(int userId) {
        var hobbies = await (from mapping in context.UserHobbies
            join hobby in context.Hobbies on mapping.HobbyId equals hobby.Id 
            where mapping.UserId == userId 
            select new HobbyDto(
                hobby.Name
            )).ToListAsync();
        
        return hobbies;
    }

    public async Task<List<LanguageDto>> GetVocalLanguagesAsync(int userId) {
        var languages = await (from mapping in context.UserVocalLanguages
            join language in context.VocalLanguages on mapping.VocalLanguageId equals language.Id 
            where mapping.UserId == userId 
            select new LanguageDto(
                language.Name,
                mapping.Level
            )).ToListAsync();
        
        return languages;
    }

    public async Task<List<ContactDetailsDto>> GetContactDetailsAsync(int userId) {
        var contactDetails = await (from userCredential in context.UserCredentials
            join user in context.Users on userCredential.UserId equals user.Id
            where user.Id == userId
            select new ContactDetailsDto(
                userCredential.Email,
                user.GithubProfileLink,
                user.LinkedInProfileLink,
                user.PhoneNumber
            )).ToListAsync();
        
        return contactDetails;
    }

    public async Task<List<ProjectDetailsDto>> GetProjectsAsync(int userId) {
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
            projectDetails.Add(new ProjectDetailsDto(userProject.Name, technologiesDictionary[userProject.Id], userProject.StartDate, userProject.EndDate, userProject.GithubRepoLink));
        }
        
        return projectDetails;
    }

    public async Task<List<EducationDetailsDto>> GetEducationDetailsAsync(int userId) {
        var educationDetails = await (from education in context.Educations where education.UserId == userId select new EducationDetailsDto(education.Study, education.InstituteName, education.State, education.Country, education.StartDate, education.EndDate)).ToListAsync();  
        return educationDetails;
    }

    public async Task<List<WorkExperienceDetailsDto>> GetWorkExperienceDetailsAsync(int userId) {
        var workExperienceDetails = await (from workExperience in context.WorkExperiences where workExperience.UserId == userId select new WorkExperienceDetailsDto(workExperience.Role, workExperience.Description, workExperience.State, workExperience.Country, workExperience.StartDate, workExperience.EndDate)).ToListAsync();  
        return workExperienceDetails;
    }
}