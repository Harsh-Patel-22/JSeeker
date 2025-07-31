using System.Security.Claims;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/application")]
[Authorize(Roles = "Hirer, Seeker")]
public class ApplicationController (JobService jobService) : ControllerBase {
    
    
    [HttpGet("get/state={state}")]
    // TODO - Limit the details of job passed around using few new DTOs. Rn on passing the job model object itself, a lot of unnecessary data is being passed and making the response heavy
    public async Task<IActionResult> GetApplicationsByStatusAsync([FromRoute] ApplicationState state) {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        
        var applications = await jobService.GetAllApplicationsByHirerIdByStateAsync(clientId, state); 
        if (applications.Count == 0) {
            return BadRequest("No applications found for the given client.");
        }
        return Ok(applications);
    }

    [HttpGet("get/{applicationId}")]
    public async Task<IActionResult> GetApplicationByIdAsync(int applicationId) {
        return Ok(await jobService.GetApplicationByIdAsync(applicationId));
    }
    
    [Authorize(Roles = "Hirer")]
    [HttpPost("status")]
    public async Task<IActionResult> UpdateApplicationStatusAsync([FromBody] ApplicationStateUpdateDto dto) {
        await jobService.UpdateApplicationStateAsync(dto);
        // Never use redirecttoaction for an api and spa...
        // return RedirectToAction("GetApplicationsByStatus", new { state = dto.State});
        return Ok();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateApplicationAsync([FromBody] CreateApplicationDto applicationDto) {
        if (await jobService.CheckAndCreateApplicationAsync(applicationDto)) {
            return Ok();
        }
        return BadRequest("Unable to create application.");
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteApplicationAsync([FromRoute] int id) {
        await jobService.DeleteApplicationAsync(id);
        return Ok();
    }
}