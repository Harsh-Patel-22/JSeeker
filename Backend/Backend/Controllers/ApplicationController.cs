using System.Security.Claims;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Extensions;
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
        Guid clientId = User.GetNameIdentifier();
        
        var applications = await jobService.GetAllApplicationsByUserIdByStateAsync(clientId, state); 
        if (applications.Count == 0) {
            return BadRequest("No applications found for the given client.");
        }
        return Ok(applications);
    }

    [HttpGet("get/{applicationId}")]
    public async Task<IActionResult> GetApplicationByIdAsync(int applicationId) {
        Guid userId = User.GetNameIdentifier();
        return Ok(await jobService.GetApplicationByIdAsync(userId, applicationId));
    }
    
    [Authorize(Roles = "Hirer")]
    [HttpPost("status")]
    public async Task<IActionResult> UpdateApplicationStatusAsync([FromBody] ApplicationStateUpdateDto dto) {
        Guid userId = User.GetNameIdentifier();
        await jobService.UpdateApplicationStateAsync(userId, dto);
        // Never use redirecttoaction for an api and spa...
        // return RedirectToAction("GetApplicationsByStatus", new { state = dto.State});
        return Ok();
    }
    
    [Authorize(Roles = "Seeker")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateApplicationAsync([FromBody]  CreateApplicationDto applicationDto) {
        if (await jobService.CheckAndCreateApplicationAsync(applicationDto)) {
            return Ok();
        }
        return BadRequest("Unable to create application.");
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteApplicationAsync([FromRoute] int id) {
        string? idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(idStr)) {
            return BadRequest("Unauthorised");
        }

        if (!Guid.TryParse(idStr, out Guid userId)) {
            return BadRequest("Unauthorised");
        }
        
        await jobService.DeleteApplicationAsync(userId, id);
        return Ok();
    }
}