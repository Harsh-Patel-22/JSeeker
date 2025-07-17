using System.Security.Claims;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/application")]
[Authorize(Roles = "Hirer, Seeker")]
public class ApplicationController (JobService jobService) : ControllerBase {
    
    
    [HttpGet("get")]
    // TODO - Limit the details of job passed around using few new DTOs. Rn on passing the job model object itself, a lot of unnecessary data is being passed and making the response heavy
    public async Task<IActionResult> GetApplicationsAsync() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        var applications = await jobService.GetAllApplicationsAsync(clientId); 
        if (applications.Count == 0) {
            return BadRequest("No applications found for the given client.");
        }
        return Ok(applications);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto applicationDto) {
        if (await jobService.CreateApplicationAsync(applicationDto)) {
            return Ok();
        }
        return BadRequest("Unable to create application.");
    }
}