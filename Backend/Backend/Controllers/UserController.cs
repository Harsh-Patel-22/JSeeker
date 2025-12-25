using System.Security.Claims;
using Backend.DTOs;
using Backend.DTOs.Users;
using Backend.DTOs.Users.Hirer;
using Backend.Extensions;
using Backend.Models.Users;
using Backend.Repositories;
using Backend.Services;
using Backend.Services.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Roles = "Hirer,Seeker")]
public class UserController (UserRepository repository, HirerService hirerService, UserService userService, JobsAggregateQueryService jobsAggregateQueryService) : ControllerBase {
    // TODO - Add edit route for editing....
    
    // Section - Registering as a Hirer
    [HttpPost("update/hirer")]
    public async Task<IActionResult> RegisterAsHirerAsync([FromBody] HirerProfessionalDetailsDto dto) {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out Guid userId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        
        await hirerService.RegisterUserAsHirerAsync(userId, dto);
        return Ok();
    }
    
    // Section - Dashboard Details
    [Authorize(Roles = "Hirer")]
    [HttpGet("get/dashboard")]
    public async Task<IActionResult> GetHirerDashboardDetailsAsync() {
        Guid userId = User.GetNameIdentifier();
        var dashboardMetrics = await jobsAggregateQueryService.GetHirerDashboardMetricsAsync(userId);
        return Ok(dashboardMetrics);
    }
    
    // Section - Filling up details
    [HttpPost("update/")]
    public async Task<IActionResult> UpdateUserSecondaryDetails([FromBody] UserSecondaryDetailsDto dto) {
        Guid userId = User.GetNameIdentifier();
        await userService.UpdateUserDetailsAsync(userId, dto);
        return Ok();
    }

    // TODO - Add checks in the workflow, Like if before the 2nd ary details are not filled, this cant be called and so on....
    [HttpPost("update/github")]
    public async Task<IActionResult> UpdateGithubUsernameAsync([FromBody] string githubUsername) {
        Guid userId = User.GetNameIdentifier();
        await userService.UpdateGithubUsernameAsync(userId, githubUsername);
        return Ok();
    }

    [HttpPost("update/repoNames")]
    public async Task<IActionResult> UpdateUserProjects([FromBody] string[] repoNames) {
        Guid userId = User.GetNameIdentifier();
        await userService.UpdateProjectsUsingGithubReposAsync(userId, repoNames.ToList());
        return Ok();
    }
    
    [HttpPost("generate/repoNames")]
    public async Task<IActionResult> UpdateUserProjectsAutoGenerate() {
        Guid userId = User.GetNameIdentifier();
        await userService.UpdateProjectsUsingGithubReposAsync(userId, null);
        return Ok();
    }

    [HttpPost("update/resume")]
    public async Task<IActionResult> RegenerateAndUpdateResumeAsync([FromBody] ResumeContentsDto resumeContents) {
        Guid userId = User.GetNameIdentifier();
        await userService.UpdateResumeAsync(userId, resumeContents);
        return Ok();
    }

    [HttpGet("get/resume")]
    public async Task<IActionResult> GetResumeContentsAsync() {
        Guid userId = User.GetNameIdentifier();
        string resumeJsonString = await userService.GetResumeJsonStringAsync(userId);
        return Ok(resumeJsonString);
    }

    [HttpPost("get/resume/pdf")]
    public async Task<IActionResult> GetResumePDFAsync([FromBody] Guid targetUserId) {
        Guid userId = User.GetNameIdentifier();
        var pdf = await userService.GetResumePdfAsync(userId, targetUserId);
        return File(pdf, "application/pdf", "resume.pdf");
    }

    [HttpGet("get/coordinates")]
    public async Task<IActionResult> GetCoordinatesAsync() {
        Guid userId = User.GetNameIdentifier();
        return Ok(await userService.GetCoordinatesAsync(userId));
    }
    
    [HttpGet("get/applicant/{applicantId}")]
    public async Task<IActionResult> GetApplicantAsync([FromRoute] Guid applicantId) {
        return Ok(await userService.GetApplicantDetailsAsync(applicantId));
    }

    [HttpGet("profile/details/seeker")]
    public async Task<IActionResult> GetSeekerProfileDetailsAsync() {
        Guid userId = User.GetNameIdentifier();
        return Ok(await userService.GetSeekerProfileDetailsAsync(userId));
    }
    
    [HttpGet("profile/details/hirer")]
    public async Task<IActionResult> GetHirerProfileDetailsAsync() {
        Guid userId = User.GetNameIdentifier();
        return Ok(await userService.GetHirerProfileDetailsAsync(userId));
    }
    
    // TODO - Make this one call. Dont fetch again and again!!
    // Section - Profile related fetching 
    [HttpGet("profile/basic")]
    public async Task<IActionResult> GetBasicDetails() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        return Ok(await repository.GetBasicDetailsAsync(clientId));
    }
    
    [HttpGet("profile/languages")]
    public async Task<IActionResult> GetClientLanguages() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        return Ok(await repository.GetVocalLanguagesAsync(clientId));
    }
    
    [HttpGet("profile/contact")]
    public async Task<IActionResult> GetClientContactDetails() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        return Ok(await repository.GetContactDetailsAsync(clientId));
    }

    [HttpGet("profile/projects")]
    public async Task<IActionResult> GetClientProjects() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        return Ok(await repository.GetProjectsAsync(clientId));
    }

    [HttpGet("profile/education")]
    public async Task<IActionResult> GetClientEducationDetails() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        return Ok(await repository.GetEducationDetailsAsync(clientId));
    }
    
    [HttpGet("profile/workexperience")]
    public async Task<IActionResult> GetClientWorkExperienceDetails() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        return Ok(await  repository.GetWorkExperienceDetailsAsync(clientId));
    }
}
