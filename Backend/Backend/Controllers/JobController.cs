using System.Security.Claims;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models.Users;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/job")]
[Authorize(Roles = "Hirer,Seeker")]
public class JobController (JobService jobService) : ControllerBase {
    
    // TODO - Give the check conditions for corner cases - not found, doesnt exist, any other. Catch all exceptions that can be created.
    
    [HttpPost("location/searchRadius={searchRadius}")]
    public async Task<IActionResult> GetNearbyJobsAsync([FromRoute] decimal searchRadius, [FromBody] JobSearchFilterDto searchFilter) {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var roleStr = User.FindFirstValue(ClaimTypes.Role);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }

        if (!Enum.TryParse(roleStr, out Roles role)) {
            throw new Exception("Invalid or missing Role claim in JWT.");
        }
        return Ok(await jobService.GetNearbyJobs(clientId, role, searchRadius, searchFilter));
    }
    
    [HttpGet("get/")]
    public async Task<IActionResult> GetRelevantJobsAsync([FromBody] JobSearchFilterDto searchFilter) {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var roleStr = User.FindFirstValue(ClaimTypes.Role);

        if (!Guid.TryParse(clientIdStr, out Guid clientId))
        {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        };
        if (!Roles.TryParse(roleStr, out Roles role)) {
            throw new Exception("Invalid or missing Role claim in JWT.");
        }
        var jobs = await jobService.GetRelevantJobsAsync(clientId, role, searchFilter);
        if (jobs is { Count: 0 }) {
            return NotFound("No jobs found");
        };
        return Ok(jobs);
    }
    
    [HttpPost("new")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto newJob) {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
        var isCreated = await jobService.CreateJobAsync(clientId, newJob);
        if (isCreated) return Created();
        
        return BadRequest("Could not create the job"); 
    }

    [HttpPost("update/{jobId}")]
    public async Task<IActionResult> UpdateJob([FromRoute] int jobId,  [FromBody] EditJobDto dto) {
        var isUpdated = await jobService.UpdateJobAsync(jobId, dto);
        if (isUpdated) return Ok();
        return BadRequest("Could not update the job");
    }
    
    [HttpGet("description/{id}")]
    public async Task<IActionResult> GetJobDescriptionById([FromRoute] int id) {
        var jobDescription = await jobService.GetJobDescriptionByIdAsync(id);
        if (jobDescription == null) {
            return NotFound();
        }
        return Ok(jobDescription);
    }
    
}