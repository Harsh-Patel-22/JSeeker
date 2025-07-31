using Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/metrics")]
public class MetricsController(MetricsRepository repository) : ControllerBase {
    
    [HttpGet("get")]
    public async Task<IActionResult> Get() {
        return Ok(await repository.GetAllMetricsAsync());
    }
}