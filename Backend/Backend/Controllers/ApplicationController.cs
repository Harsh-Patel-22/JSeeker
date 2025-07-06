using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/application")]
[Authorize(Roles = "Hirer, Seeker")]
public class ApplicationController (JobService jobService) : ControllerBase {
    
    
    [HttpGet("get/clientId={clientId}")]
    // TODO - Limit the details of job passed around using few new DTOs. Rn on passing the job model object itself, a lot of unnecessary data is being passed and making the response heavy
    public async Task<IActionResult> GetApplicationsAsync([FromRoute] Guid clientId) {
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