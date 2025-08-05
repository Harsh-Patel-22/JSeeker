using System.Security.Claims;
using Backend.DTOs;
using Backend.Extensions;
using Backend.Models.Users;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/interview")]
[Authorize(Roles = "Hirer,Seeker")]
public class InterviewController (JobService jobService) : ControllerBase {
    [HttpGet("get/scheduled={scheduled}")]
    public async Task<IActionResult> GetInterviewsAsync([FromRoute] bool scheduled) {
        Guid userId = User.GetNameIdentifier();
        var interviews = await jobService.GetInterviewsByIdByScheduleStatusAsync(userId,  scheduled);
        return Ok(interviews);
    }

    [HttpPatch("updateDateTime/{id}")]
    public async Task<IActionResult> UpdateInterviewDateTimeAsync([FromRoute] int interviewId, [FromBody] DateAndTimeDto dto) {
        var roleStr = User.FindFirstValue(ClaimTypes.Role);
        if (!Enum.TryParse(roleStr, out Roles role)) {
            throw new Exception("Invalid or missing Role claim in JWT.");
        }

        Guid userId = User.GetNameIdentifier();
        await jobService.UpdateInterviewDateTimeAsync(userId, interviewId, role, dto);
        return NoContent();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateInterviewAsync([FromBody] CreateInterviewDto interviewDto) {
        if (await jobService.CreateInterviewAsync(interviewDto)) {
            return Ok();
        }
        return BadRequest("Failed to create interview");
    }

    [HttpGet("get/{interviewId}")]
    public async Task<IActionResult> GetInterviewByIdAsync(int interviewId) {
        Guid userId = User.GetNameIdentifier();
        return Ok(await jobService.GetInterviewByIdAsync(userId, interviewId));
    }
    
}