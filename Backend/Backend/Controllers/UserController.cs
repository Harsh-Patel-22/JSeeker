using System.Security.Claims;
using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Roles = "Hirer,Seeker")]
public class UserController (UserRepository repository) : ControllerBase {
    
    [HttpGet("profile/basic")]
    public async Task<IActionResult> GetBasicDetails() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        return Ok(await repository.GetBasicDetailsAsync(clientId));
    }

    [HttpGet("profile/hobbies")]
    public async Task<IActionResult> GetClientHobbies() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        return Ok(await repository.GetHobbiesAsync(clientId));
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
