using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/job")]
public class JobController : ControllerBase {
    private readonly ApplicationDbContext _context;
    
    // TODO - Give the check conditions for corner cases - not found, doesnt exist, any other. Catch all exceptions that can be created.
    public JobController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpPost("location")]
    public IActionResult GetNearbyJobs([FromBody] SearchLocationDto searchLocationDto) {
        decimal searchDistance = searchLocationDto.SearchDistance;
        
        // TODO - Set the diagonal checking in the distance. The total distance should be within the search ditance and not just in terms of latitude and longitude separately
        List<JobForMapMarkerDto> nearbyJobs = (from job in _context.Jobs
            join location in _context.Locations
                on job.LocationId equals location.Id where Math.Abs(location.Latitude - searchLocationDto.Latitude) < searchDistance && Math.Abs(location.Longitude - searchLocationDto.Longitude) < searchDistance
            select new JobForMapMarkerDto()
            {
                Id = job.Id,
                Title = job.Title,
                Distance = (decimal) Math.Sqrt(Math.Pow((double) (location.Latitude - searchLocationDto.Latitude), 2) + Math.Pow((double) (location.Latitude - searchLocationDto.Latitude), 2)),
                Location = location
            }).ToList();
        return Ok(nearbyJobs);
        
        // DONE
        // TODO - Smart filter based on the distance in the query itself - DONE!
        // TODO - Rather than returning a list of locations, return a list of jobs; Every detail for the job. - DONE!
    }
    
    [HttpGet("get/clientId={clientId}")]
    public IActionResult GetRelevantJobs([FromRoute] int clientId) {
        Console.WriteLine(clientId);
        List<Job> relevantJobs = _context.Jobs.ToList();
        List<JobCardDto> relevantJobCards = new List<JobCardDto>();
        
        foreach (Job job in relevantJobs) {
            relevantJobCards.Add(new JobCardDto() {
                Id = job.Id,
                Title = job.Title,
                Status = job.Status,
                WorkMode = "On-Site",
                Location = (from location in _context.Locations where location.Id == job.LocationId select location).First(),
                MinSalary = job.Salary,
                MaxSalary = job.Salary + 100000,
            });
        }
        
        // TODO - Add the code to filter jobs based on the hirer's posts (belonging to the hirer) and seeker's interest (based on skills and experience)
        return Ok(relevantJobCards);
    }
    
    [HttpPost("new")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto newJob) {
        await _context.Jobs.AddAsync(new Job( _context.Jobs.Count(),newJob.Title, newJob.Description, newJob.TermsAndConditions, newJob.Salary, newJob.Status, newJob.LocationId) {
            Title = string.Empty,
            Description = string.Empty
        });    
        await _context.SaveChangesAsync();
        
        // TODO - Configure the new job object from the CreateJobDTO. - DONE
        // TODO - Refactor the Job Table and Other respective classes
        return Created();
    }
    
    [HttpGet("description/{id}")]
    public async Task<IActionResult> GetJobDescriptionById([FromRoute] int id) {
        Job? job = await _context.Jobs.FindAsync(id);
        if(job == null) {
            return BadRequest();
        }
        
        return Ok(new JobDescriptionDto() {
            Id = job.Id,
            Title = job.Title,
            Status = job.Status,
            WorkMode = "On-Site",
            MinSalary = job.Salary,
            MaxSalary = job.Salary + 100000,
            Location = (from location in _context.Locations where location.Id == job.LocationId select location).First(),
            
            Description = job.Description,
            TermsAndConditions = job.TermsAndConditions,
            Requirements = "Requirements",
            Miscellaneous = "lorem32",
            
            PostedDaysAgo = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
            NumberOfApplicants = _context.Applications.Count(),
        });
    }
    
}