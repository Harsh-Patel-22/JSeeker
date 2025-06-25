using Backend.Data;
using Backend.DTOs.Users;
using Backend.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase {
    private readonly ApplicationDbContext _context;
    public UserController(ApplicationDbContext context) {
        _context = context;
    }
    
    [HttpGet("get/clientId={clientId}")]
    public IActionResult GetUserById([FromRoute] int clientId) {
        
        return Ok();
    }
    
    [HttpGet("profile/clientId={clientId}&role={role}")]
    public IActionResult GetUserProfile([FromRoute] int clientId, [FromRoute] Role role) {
        switch (role) {
            case Role.Hirer:
                var hirerData = from user in _context.Users
                    join hirer in _context.Hirers on user.Id equals hirer.Id
                    join address in _context.Addresses on hirer.CompanyAddressId equals address.Id
                    where user.Id == clientId
                    select new HirerProfileDto(
                        user.FirstName,
                        user.LastName,
                        user.PhoneNumber,
                        user.Gender,
                        address
                        ); 
                    
                return Ok(hirerData);
            
            case Role.Seeker:
                var seekerData = from user in _context.Users join seeker in _context.Seekers on user.Id equals seeker.Id join address in _context.Addresses on seeker.AddressId equals address.Id where user.Id == clientId select new SeekerProfileDto() {
                    Address = address,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    GithubUsername = seeker.GithubUsername,
                    WorkExperienceInYears = seeker.WorkExperienceInYears,
                    ResumeURL = seeker.ResumeUrl,
                };
                return Ok(seekerData);
        }

        return BadRequest();
    }
    [HttpGet]
    public IActionResult GetBasicDetails() {
        return Ok();
    }
}
