using System.Security.Claims;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Extensions;
using Backend.Models;
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
        
        var roleStr = User.FindFirstValue(ClaimTypes.Role);
        Guid userId = User.GetNameIdentifier();
        if (!Enum.TryParse(roleStr, out Roles role)) {
            throw new Exception("Unauthorised");
        }
        return Ok(await jobService.GetNearbyJobs(userId, role, searchRadius, searchFilter));
    }
    
    [HttpPost("get/")]
    public async Task<IActionResult> GetRelevantJobsAsync([FromBody] JobSearchFilterDto searchFilter) {
        var roleStr = User.FindFirstValue(ClaimTypes.Role);
        Guid userId = User.GetNameIdentifier();
        if (!Roles.TryParse(roleStr, out Roles role)) {
            throw new Exception("Unauthorised");
        }
        var jobs = await jobService.GetRelevantJobsAsync(userId, role, searchFilter);
        if (jobs is { Count: 0 }) {
            return NotFound("No jobs found");
        };
        return Ok(jobs);
    }

    [HttpGet("get/applied")]
    public async Task<IActionResult> GetAppliedJobsAsync() {
        var userId = User.GetNameIdentifier();
        var jobs = await jobService.GetAppliedJobsAsync(userId);
        if (jobs != null && jobs.Count > 0) {
            return Ok(jobs);
        }
        return NotFound("No jobs found");
    }
    
    [HttpPost("new")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto newJob) {
        Guid userid = User.GetNameIdentifier();
        var isCreated = await jobService.CreateJobAsync(userid, newJob);
        if (isCreated) return Created();
        
        return BadRequest("Could not create the job"); 
    }

    [HttpPost("update/{jobId}")]
    public async Task<IActionResult> UpdateJob([FromRoute] int jobId,  [FromBody] EditJobDto dto) {
        Guid userid = User.GetNameIdentifier();
        var isUpdated = await jobService.UpdateJobAsync(userid, jobId, dto);
        if (isUpdated) return Ok();
        return BadRequest("Could not update the job");
    }

    [HttpPost("update/status/{jobId}")]
    public async Task<IActionResult> UpdateJobStatus([FromRoute] int jobId, [FromBody] JobStatus jobStatus) {
        Guid userid = User.GetNameIdentifier();
        var isUpdated = await jobService.UpdateJobStatusAsync(userid, jobId, jobStatus);
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