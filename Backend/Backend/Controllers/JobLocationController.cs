using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/joblocation")]
public class JobLocationController : ControllerBase {
    private readonly ApplicationDbContext _context;
    
    public JobLocationController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpPost]
    public IActionResult GetNearbyJobs([FromBody] SearchLocationDTO searchLocationDto) {
        decimal searchDistance = searchLocationDto.SearchDistance;
        List<Location> locations =  _context.Locations.ToList();
        List<Location> targetLocations = new List<Location>();
        foreach (var location in locations) {
            if(Math.Abs(location.Latitude - searchLocationDto.Latitude) < searchDistance && Math.Abs(location.Longitude - searchLocationDto.Longitude) < searchDistance) {
                targetLocations.Add(location);
            }
        }
        return Ok(targetLocations);
        // TODO - Rather than returning a list of locations, return a list of jobs; Every detail for the job.
    }
}