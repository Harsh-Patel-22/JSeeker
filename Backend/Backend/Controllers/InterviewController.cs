using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/interview")]
[Authorize(Roles = "Hirer,Seeker")]
public class InterviewController (JobService jobService) : ControllerBase {
    [HttpGet("get/clientId={clientId}")]
    public async Task<IActionResult> GetInterviewsAsync([FromRoute] Guid clientId) {
        var interviews = await jobService.GetInterviewsByIdAsync(clientId);
        return Ok(interviews);
    }
    
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateInterview([FromBody] CreateInterviewDto interviewDto) {
        if (await jobService.CreateInterviewAsync(interviewDto)) {
            return Ok();
        }
        return BadRequest("Failed to create interview");
    }
    
}