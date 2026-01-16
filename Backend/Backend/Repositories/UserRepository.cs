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
        
        await DbUpdateHelper.UpdateAllFieldsExceptAsync(h, context, "Id");
        await context.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(setter => setter.SetProperty(user => user.IsHirer, true));
    }

    public async Task UpdateUserDetailsWithResumeDtoAsync(Guid userId, UpdatedResumeContentsDto dto) {
        foreach (var experienceDetails in dto.WorkExperienceDetails) {

            if (experienceDetails.Id == -1) {
                // Add new work experience
                await context.WorkExperiences.AddAsync(new WorkExperience() {
                    Role = experienceDetails.Role,
                    Description = experienceDetails.Description,
                    CompanyName = experienceDetails.CompanyName,
                    StartDate = experienceDetails.StartDate,
                    EndDate = experienceDetails.EndDate,
                });
                await context.SaveChangesAsync();
            }
            else {
                await context.WorkExperiences.Where(we => we.Id == experienceDetails.Id).ExecuteUpdateAsync(setter => setter.SetProperty(we => we.CompanyName, experienceDetails.CompanyName).SetProperty(we => we.Description, experienceDetails.Description).SetProperty(we => we.Role, experienceDetails.Role).SetProperty(we => we.EndDate, experienceDetails.EndDate).SetProperty(we => we.StartDate, experienceDetails.StartDate));
            }
        }
        foreach (var educationDetails in dto.EducationDetails) {

            if (educationDetails.Id == -1) {
                // Add new education
                await context.Educations.AddAsync(new Education() {
                    Study = educationDetails.Study,
                    InstituteName = educationDetails.InstituteName,
                    State = educationDetails.State,
                    Country = educationDetails.Country,
                    EndDate = educationDetails.EndDate,
                    StartDate = educationDetails.StartDate
                });
                await context.SaveChangesAsync();
            }
            else {
                await context.Educations.Where(e => e.Id == educationDetails.Id).ExecuteUpdateAsync(setter => setter.SetProperty(e => e.Country, educationDetails.Country).SetProperty(e => e.InstituteName, educationDetails.InstituteName).SetProperty(e => e.Study, educationDetails.Study).SetProperty(e => e.State, educationDetails.State).SetProperty(e => e.EndDate, educationDetails.EndDate).SetProperty(e => e.StartDate, educationDetails.StartDate));
            }
        }

        foreach (var languageDetails in dto.LanguageDetails) {
            var languageId = await GetLanguageIdByNameAsync(languageDetails.Name);
            int recordsUpdated = await context.UserVocalLanguages.Where(uvl => uvl.UserId == userId && uvl.VocalLanguageId == languageId).ExecuteUpdateAsync(setter => setter.SetProperty(uvl => uvl.Level, languageDetails.Level));

            if (recordsUpdated == 0) {
                // A new entry to the linking table
                await context.UserVocalLanguages.AddAsync(new UserVocalLanguage() {
                    UserId = userId,
                    VocalLanguageId = languageId,
                    Level = languageDetails.Level
                });

            }
        }


        foreach (var deletedEducation in dto.DeletedEducationDetails) {
            await context.Educations.Where(e => e.Id == deletedEducation.Id).ExecuteDeleteAsync();
        }
        foreach (var deletedWorkExperience in dto.DeletedWorkExperienceDetails) {
            await context.WorkExperiences.Where(we => we.Id == deletedWorkExperience.Id).ExecuteDeleteAsync();
        }
    }
    
    public async Task UpdateUserSecondaryDetailsAsync(Guid userId, UserSecondaryDetailsDto dto) {
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
        // TODO - Remove the existing education/project-related/work experience related details and add the new ones.... Or simply, take those objects with id and overwrite them with the current ones..
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
        await DbUpdateHelper.UpdateAllFieldsExceptAsync(u, context, "Id", "PhoneNumber", "Email", "FirstName", "LastName", "GithubUsername", "NumberOfSuccessfulEmployments", "NumberOfRejections", "TechnicalKeywords", "AIGeneratedTechnicalKeywords", "ResumeJsonString", "ResumeTemplateNumber");
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
    
    public async Task<string> GetResumeJsonStringAsync(Guid userId) {
        return await context.Users.Where(user => user.Id == userId).Select(user => user.ResumeJsonString).FirstOrDefaultAsync();
    }

    public async Task<AddressCoordinatesDto> GetCoordinatesAsync(Guid userId) {
        return await context.Users.Where(user => user.Id == userId).Include(user => user.Address).Select(user => new AddressCoordinatesDto() {
            Latitude = user.Address.Latitude,
            Longitude = user.Address.Longitude,
        }).FirstOrDefaultAsync();
    }
    
    public async Task<ApplicantDetailsDto> GetApplicantDetailsAsync(Guid userId) {
        return await context.Users.Where(user => user.Id == userId).Select(user => new ApplicantDetailsDto() {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            // Skills = user.TechnicalKeywords.Split(',').ToList(),
        }).FirstOrDefaultAsync();
    }

    public async Task<ResumeTemplateAndStringDto> GetResumeAndTemplateNumberAsync(Guid userId) {
        return await context.Users.Where(user => user.Id == userId).Select(user => new ResumeTemplateAndStringDto() {
            ResumeString = user.ResumeJsonString,
            TemplateNumber = user.ResumeTemplateNumber
        }).FirstOrDefaultAsync();
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
            projectDetails.Add(project.Name, new ProjectDetailsDto(project.Id, project.Name, project.Description, usage, project.StartDate, project.LastUpdatedDate, project.GithubRepoLink));
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
                setter.SetProperty(user => user.NumberOfRejections,
                    user => user.NumberOfRejections + 1));
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }
    
    // Section - Profile Related
    public async Task<SeekerProfileDetailsDto> GetSeekerProfileDetailsAsync(Guid userId) {
        SeekerProfileDetailsDto dto = new SeekerProfileDetailsDto();
        dto.BasicDetails =  await GetBasicDetailsAsync(userId);
        dto.EducationDetails =  await GetEducationDetailsAsync(userId);
        dto.ProjectDetails =  await GetProjectsAsync(userId);
        dto.VocalLanguage =  await GetVocalLanguagesAsync(userId);
        dto.WorkExperienceDetails = await GetWorkExperienceDetailsAsync(userId);
        dto.ContactDetails = await GetContactDetailsAsync(userId);
        return dto;
    }
    public async Task<HirerProfileDetailsDto> GetHirerProfileDetailsAsync(Guid userId) {
        var companyDetails = await context.Hirers.Where(h => h.Id == userId).Include(h => h.CompanyAddress).Select(h => new { h.CompanyName, h.Designation, h.WebsiteLink, h.CompanyAddress }).FirstOrDefaultAsync();
        var dto = await context.Users.Where(u => u.Id == userId).Include(u => u.Address).Select(u => new HirerProfileDetailsDto() {
            FirstName = u.FirstName,
            LastName = u.LastName,
            PhoneNumber = u.PhoneNumber,
            CompanyName =  companyDetails.CompanyName,
            Designation =  companyDetails.Designation,
            CompanyAddress = companyDetails.CompanyAddress,
            Gender =  u.Gender,
            WebsiteLink = companyDetails.WebsiteLink,
        }).FirstOrDefaultAsync();
        return dto;
    }
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
            projectDetails.Add(new ProjectDetailsDto( userProject.Id, userProject.Name, userProject.Description, technologiesDictionary[userProject.Id], userProject.StartDate, userProject.LastUpdatedDate, userProject.GithubRepoLink));
        }
        
        return projectDetails;
    }

    public async Task<List<EducationDetailsDto>> GetEducationDetailsAsync(Guid userId) {
        // var educationDetails = await (from education in context.Educations where education.UserId == userId select new EducationDetailsDto(education.Study, education.InstituteName, education.State, education.Country, education.StartDate, education.EndDate)).ToListAsync();  
        var educationDetails = await context.Educations.Where(education => education.UserId == userId).OrderByDescending(we => we.StartDate).Select(e => new EducationDetailsDto(e.Id, e.Study, e.InstituteName, e.State, e.Country, e.StartDate, e.EndDate)).ToListAsync();
        return educationDetails;
    }

    public async Task<List<WorkExperienceDetailsDto>> GetWorkExperienceDetailsAsync(Guid userId) {
        var workExperienceDetails = await context.WorkExperiences.Where(workExperience => workExperience.UserId == userId).OrderByDescending(we => we.StartDate).Select(we => new WorkExperienceDetailsDto(we.Id, we.Role, we.Description, we.CompanyName, we.StartDate, we.EndDate)).ToListAsync();
        // var workExperienceDetails = await (from workExperience in context.WorkExperiences where workExperience.UserId == userId select new WorkExperienceDetailsDto(workExperience.Role, workExperience.Description, workExperience.CompanyName, workExperience.StartDate, workExperience.EndDate)).ToListAsync();  
        return workExperienceDetails;
    }
}