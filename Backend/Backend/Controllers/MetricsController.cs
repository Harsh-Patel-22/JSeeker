using Backend.Repositories;
using Backend.Services.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/metrics")]
public class MetricsController(MetricsQueryService queryService) : ControllerBase {
    
    [HttpGet("get")]
    public async Task<IActionResult> Get() {
        return Ok(await queryService.GetAllMetricsAsync());
    }
}