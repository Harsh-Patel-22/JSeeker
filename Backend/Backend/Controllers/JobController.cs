using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/joblocation")]
public class JobController : ControllerBase {
    private readonly ApplicationDbContext _context;
    
    public JobController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpPost]
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
        /*
        List<JobDTO> nearbyJobs = new List<JobDTO>();
        foreach (var job in allJobs) {
            if(Math.Abs(job.Location.Latitude - searchLocationDto.Latitude) < searchDistance && Math.Abs(job.Location.Longitude - searchLocationDto.Longitude) < searchDistance) {
                nearbyJobs.Add(job);
            }
        }
*/
        return Ok(nearbyJobs);
        // TODO - Smart filter based on the distance in the query itself

        // TODO - Rather than returning a list of locations, return a list of jobs; Every detail for the job. - DONE!
    }
}