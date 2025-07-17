using System.Security.Claims;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/interview")]
[Authorize(Roles = "Hirer,Seeker")]
public class InterviewController (JobService jobService) : ControllerBase {
    [HttpGet("get")]
    public async Task<IActionResult> GetInterviewsAsync() {
        var clientIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(clientIdStr, out Guid clientId)) {
            throw new Exception("Invalid or missing NameId claim in JWT.");
        }
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