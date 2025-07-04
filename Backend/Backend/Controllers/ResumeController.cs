using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/resume")]
[Authorize(Roles = "Hirer,Seeker")]
public class ResumeController (AIService service) : ControllerBase {
    
    [HttpGet("prompt={prompt}")]
    public async Task<IActionResult> TestPrompt([FromRoute] string prompt) {
        return Ok(await service.GetChatResponseAsync(prompt));
    }
}