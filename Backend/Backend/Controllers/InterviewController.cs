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
[Route("api/interview")]
[Authorize(Roles = "Hirer,Seeker")]
public class InterviewController (JobService jobService) : ControllerBase {
    [HttpGet("get/state={state}")]
    public async Task<IActionResult> GetInterviewsAsync([FromRoute] InterviewState state) {
        Guid userId = User.GetNameIdentifier();
        Roles role = User.GetRole();
        var interviews = await jobService.GetInterviewsByIdByScheduleStatusAsync(userId, role, state);
        return Ok(interviews);
    }

    [HttpPost("update/DateTime/{interviewId}")]
    public async Task<IActionResult> UpdateInterviewDateTimeAsync([FromRoute] int interviewId, [FromBody] DateAndTimeDto dto) {
        var roleStr = User.FindFirstValue(ClaimTypes.Role);
        if (!Enum.TryParse(roleStr, out Roles role)) {
            throw new Exception("Invalid or missing Role claim in JWT.");
        }

        Guid userId = User.GetNameIdentifier();
        await jobService.UpdateInterviewDateTimeAsync(userId, interviewId, role, dto);
        return Ok();
    }

    [HttpPost("scheduled/{id}")]
    public async Task<IActionResult> SetInterviewScheduled([FromRoute] int id) {
        Guid userId = User.GetNameIdentifier();
        Roles role = User.GetRole();
        await jobService.SetInterviewScheduledAsync(userId, role, id);
        return Ok();
    }
    
    [Authorize(Roles = "Hirer")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateInterviewAsync([FromBody] CreateInterviewDto interviewDto) {
        if (await jobService.CreateInterviewAsync(interviewDto)) {
            return Ok();
            
        }
        return BadRequest("Failed to create interview");
    }

    [HttpPost("update/success/{id}")]
    public async Task<IActionResult> UpdateSuccessStatusAsync([FromRoute] int id, [FromBody] InterviewOutcome outcome) {
        Guid userId = User.GetNameIdentifier();
        await jobService.UpdateSeekerSuccessFailureJobLandingAsync(userId, id, outcome);
        return Ok();
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetInterviewByIdAsync(int id) {
        Guid userId = User.GetNameIdentifier();
        return Ok(await jobService.GetInterviewByIdAsync(userId, id));
    }
    
}