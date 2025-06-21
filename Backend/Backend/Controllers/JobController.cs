using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobController : ControllerBase {
    private readonly ApplicationDbContext _context;
    
    public JobController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpPost("location")]
    public IActionResult GetNearbyJobs([FromBody] SearchLocationDTO searchLocationDto) {
        decimal searchDistance = searchLocationDto.SearchDistance;
        
        List<JobDTO> nearbyJobs = (from job in _context.Jobs
            join location in _context.Locations
                on job.LocationId equals location.Id where Math.Abs(location.Latitude - searchLocationDto.Latitude) < searchDistance && Math.Abs(location.Longitude - searchLocationDto.Longitude) < searchDistance
            select new JobDTO()
            {
                Title = job.Title,
                Description = job.Description,
                TermsAndConditions = job.TermsAndConditions,
                Salary = job.Salary,
                Location = location,
                
            }).ToList();
        return Ok(nearbyJobs);
        
        // DONE
        // TODO - Smart filter based on the distance in the query itself - DONE!
        // TODO - Rather than returning a list of locations, return a list of jobs; Every detail for the job. - DONE!
    }
    
    [HttpPost]
    public IActionResult GetRelevantJobs([FromBody] ClientIdDTO clientIdDto) {
        // TODO - Add the code to filter jobs based on the hirer's posts (belonging to the hirer) and seeker's interest (based on skills and experience)
        return Ok(_context.Jobs.ToList());
    }
    
    [HttpPost("new")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDTO newJob) {
        await _context.Jobs.AddAsync(new Job( _context.Jobs.Count(),newJob.Title, newJob.Description, newJob.TermsAndConditions, newJob.Salary, newJob.Status, newJob.LocationId) {
            Title = string.Empty,
            Description = string.Empty
        });    
        await _context.SaveChangesAsync();
        
        // TODO - Configure the new job object from the CreateJobDTO. - DONE
        // TODO - Refactor the Job Table and Other respective classes
        return Created();
    }
    
    [HttpGet("get/{id}")]
    public IActionResult GetJobById([FromRoute] int id) {
        return Ok(_context.Jobs.Find(id));
    }
    
}