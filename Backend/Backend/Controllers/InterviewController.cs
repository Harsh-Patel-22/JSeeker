using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/interview")]
public class InterviewController : ControllerBase {
    private readonly ApplicationDbContext  _context;

    public InterviewController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpPost("get")]
    public async Task<IActionResult> GetInterviews([FromBody] ClientIdDTO clientIdDto) {
        var interviews = 
            from interview in _context.Interviews
            join job in _context.Jobs on interview.JobId equals job.Id
            join location in _context.Locations on job.LocationId equals location.Id
            where clientIdDto.Id == interview.HirerId || clientIdDto.Id == interview.SeekerId 
            select new InterviewDTO() {
            SeekerId = interview.SeekerId,
            HirerId = interview.HirerId,
            JobId = interview.JobId,
            Job = new JobDTO() {
                Title = job.Title, 
                Description = job.Description,
                TermsAndConditions = job.TermsAndConditions,
                Salary = job.Salary, 
                Location = location
            } ,
            Date = interview.Date,
            Time = interview.Time,
            Mode = interview.Mode,
            };  
        
        return Ok(interviews.ToList());
    }
    
    [HttpGet("get")]
    public IActionResult GetAllInterviews() {
        return Ok(_context.Interviews.ToList());
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateInterview([FromBody] CreateInterviewDTO interviewDto) {
        await _context.Interviews.AddAsync(new Interview() {
            Date = interviewDto.Date,
            Time = interviewDto.Time,
            Mode = interviewDto.Mode,
            HirerId = interviewDto.HirerId,
            SeekerId = interviewDto.SeekerId,
            JobId = interviewDto.JobId
        });
        await _context.SaveChangesAsync();
        return Ok();
    }
    
}