using Backend.Data;
using Backend.DTOs.Users;
using Backend.Models.Users;
using Backend.Models.Users.WorkRelated;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController (UserRepository repository) : ControllerBase {
    
    [HttpGet("profile/basic/clientId={clientId}")]
    public async Task<IActionResult> GetBasicDetails([FromRoute] int clientId) {
        return Ok(await repository.GetBasicDetailsAsync(clientId));
    }

    [HttpGet("profile/hobbies/clientId={clientId}")]
    public async Task<IActionResult> GetClientHobbies([FromRoute] int clientId) {
        return Ok(await repository.GetHobbiesAsync(clientId));
    }
    
    [HttpGet("profile/languages/clientId={clientId}")]
    public async Task<IActionResult> GetClientLanguages([FromRoute] int clientId) {
        return Ok(await repository.GetVocalLanguagesAsync(clientId));
    }
    
    [HttpGet("profile/contact/clientId={clientId}")]
    public async Task<IActionResult> GetClientContactDetails([FromRoute] int clientId) {
        return Ok(await repository.GetContactDetailsAsync(clientId));
    }

    [HttpGet("profile/projects/clientId={clientId}")]
    public async Task<IActionResult> GetClientProjects([FromRoute] int clientId) {
        return Ok(await repository.GetProjectsAsync(clientId));
    }

    [HttpGet("profile/education/clientId={clientId}")]
    public async Task<IActionResult> GetClientEducationDetails([FromRoute] int clientId) {
        return Ok(await repository.GetEducationDetailsAsync(clientId));
    }
    
    [HttpGet("profile/workexperience/clientId={clientId}")]
    public async Task<IActionResult> GetClientWorkExperienceDetails([FromRoute] int clientId) {
        return Ok(await  repository.GetWorkExperienceDetailsAsync(clientId));
    }
}
