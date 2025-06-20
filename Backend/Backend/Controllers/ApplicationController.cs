using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/application")]
public class ApplicationController : ControllerBase {
    private readonly ApplicationDbContext _context;

    public ApplicationController(ApplicationDbContext context) {
        _context = context;
    }
    
    
    [HttpPost("get")]
    // TODO - Limit the details of job passed around using few new DTOs. Rn on passing the job model object itself, a lot of unnecessary data is being passed and making the response heavy
    public IActionResult GetApplications([FromBody] ClientIdDTO clientIdDto) {
        var applications = from application in _context.Applications
            where application.HirerId == clientIdDto.Id 
            select new ApplicationDTO() {
            ApplicantId = application.ApplicantId,
            JobId = application.JobId,
            HirerId = application.HirerId,
        };
        return Ok(applications.ToList());
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDTO applicationDto) {
        await _context.Applications.AddAsync(new Application() {
            ApplicantId = applicationDto.ApplicantId,
            JobId = applicationDto.JobId,
            HirerId = applicationDto.HirerId,
        });
        return Ok();
    }
}