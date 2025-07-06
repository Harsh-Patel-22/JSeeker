using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/job")]
[Authorize(Roles = "Hirer,Seeker")]
public class JobController (JobService jobService) : ControllerBase {
    
    // TODO - Give the check conditions for corner cases - not found, doesnt exist, any other. Catch all exceptions that can be created.
    
    [HttpPost("location")]
    public async Task<IActionResult> GetNearbyJobsAsync([FromBody] SearchLocationDto searchLocationDto) {
        return Ok(await jobService.GetNearbyJobs(searchLocationDto));
    }
    
    [HttpGet("get/clientId={clientId}")]
    public async Task<IActionResult> GetRelevantJobs([FromRoute] Guid clientId) {
        var jobs = await jobService.GetRelevantJobsAsync(clientId);
        if (jobs.Count == 0) {
            return NotFound("No jobs found");
        };
        return Ok(jobs);
    }
    
    [HttpPost("new")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto newJob) {
        var isCreated = await jobService.CreateJobAsync(newJob);
        if (isCreated) return Created();
        
        return BadRequest(); 
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