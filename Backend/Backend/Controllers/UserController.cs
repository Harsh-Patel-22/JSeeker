using Backend.Data;
using Backend.DTOs.Users;
using Backend.Models.Users;
using Backend.Models.Users.WorkRelated;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase {
    private readonly ApplicationDbContext _context;
    public UserController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpGet("profile/basic/clientId={clientId}")]
    public IActionResult GetBasicDetails([FromRoute] int clientId) {
        var details = from user in _context.Users
            join address in _context.Addresses on user.AddressId equals address.Id 
            where user.Id == clientId
            select new BasicDetailsDto(
                user.FirstName, 
                user.LastName, 
                user.Occupation, 
                address.State, 
                address.Country, 
                user.AboutLine
                );
        
        return Ok(details);
    }

    [HttpGet("profile/hobbies/clientId={clientId}")]
    public IActionResult GetClientHobbies([FromRoute] int clientId) {
        var hobbies = from mapping in _context.UserHobbies
            join hobby in _context.Hobbies on mapping.HobbyId equals hobby.Id 
            where mapping.UserId == clientId 
            select new HobbyDto(
                hobby.Name
                );
        return Ok(hobbies);
    }
    
    [HttpGet("profile/languages/clientId={clientId}")]
    public IActionResult GetClientLanguages([FromRoute] int clientId) {
        var languages = from mapping in _context.UserVocalLanguages
            join language in _context.VocalLanguages on mapping.VocalLanguageId equals language.Id 
            where mapping.UserId == clientId 
            select new LanguageDto(
                language.Name,
                mapping.Level
            );
        return Ok(languages);
    }
    
    [HttpGet("profile/contact/clientId={clientId}")]
    public IActionResult GetClientContactDetails([FromRoute] int clientId) {
        var contactDetails = from userCredential in _context.UserCredentials join user in _context.Users
            on  userCredential.UserId equals user.Id
            where user.Id == clientId
            select new ContactDetailsDto(
                userCredential.Email,
                user.GithubProfileLink,
                user.LinkedInProfileLink,
                user.PhoneNumber
            );
        return Ok(contactDetails);
    }

    [HttpGet("profile/projects/clientId={clientId}")]
    public IActionResult GetClientProjects([FromRoute] int clientId) {

        List<Project> userProjectModels = _context.Projects.Where(project => project.UserId == clientId).ToList();
        Dictionary<int,List<TechnologyUsageDto>> technologiesDictionary = new Dictionary<int, List<TechnologyUsageDto>>();
        foreach (var userProject in userProjectModels) {
            var technologiesList = from projectTechnology in _context.ProjectTechnologies
                join technology in _context.Technologies on projectTechnology.TechnologyId equals technology.Id where projectTechnology.ProjectId == userProject.Id select new TechnologyUsageDto(technology.Name, projectTechnology.PercentUsage);
            technologiesDictionary.Add(userProject.Id, technologiesList.ToList());
        }
        
        List<ProjectDetailsDto> projectDetails = new List<ProjectDetailsDto>();
        foreach (var userProject in userProjectModels) {
            projectDetails.Add(new ProjectDetailsDto(userProject.Name, technologiesDictionary[userProject.Id], userProject.StartDate, userProject.EndDate, userProject.GithubRepoLink));
        }
        
        return Ok(projectDetails);
    }

    [HttpGet("profile/education/clientId={clientId}")]
    public IActionResult GetClientEducationDetails([FromRoute] int clientId) {
        var educationDetails = from education in _context.Educations where education.UserId == clientId select new EducationDetailsDto(education.Study, education.InstituteName, education.State, education.Country, education.StartDate, education.EndDate);  
        return Ok(educationDetails);
    }
    
    [HttpGet("profile/workexperience/clientId={clientId}")]
    public IActionResult GetClientWorkExperienceDetails([FromRoute] int clientId) {
        var workExperienceDetails = from workExperience in _context.WorkExperiences where workExperience.UserId == clientId select new WorkExperienceDetailsDto(workExperience.Role, workExperience.Description, workExperience.State, workExperience.Country, workExperience.StartDate, workExperience.EndDate);  
        return Ok(workExperienceDetails);
    }

}
