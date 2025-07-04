using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Testing;

[ApiController]
[Route("api/testing")]
public class TestingController(ResumeBuilderService resumeBuilder) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> Get() {
        var response = await resumeBuilder.GetResumeDescription();
        return Ok(response);
    }
}