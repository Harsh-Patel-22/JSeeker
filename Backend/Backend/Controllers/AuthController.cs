using Backend.DTOs;
using Backend.DTOs.Users;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase {

    [HttpPost("login")]
    // ReSharper disable once InconsistentNaming
    public async Task<IActionResult> VerifyLoginCredentials([FromBody] LoginCredentialsDto credentials) {
        string jwtToken = await authService.LoginUserAsync(credentials);
        if(jwtToken.Equals(string.Empty)) {
            return BadRequest("Invalid credentials");
        }
        return Ok(jwtToken);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterNewUser([FromBody] UserPrimaryDetailsFillUpDto credentials) {
        string jwtToken = await authService.RegisterNewUserAsync(credentials);
        if (jwtToken.Equals(string.Empty)) {
            return BadRequest("User already registered!");
        }
        return Ok(jwtToken);
    }
    
    // Async Calls from frontend - Checker Functions
    [HttpPost("available/username")]
    public async Task<IActionResult> UsernameAvailabilityVerifier([FromBody] string username) {
        string response = await authService.CheckIfUsernameOrEmailExistsAsync(username);
        if (response.Equals(string.Empty)) {
            return BadRequest("Username already taken!");
        }
        return NoContent();
    }
    
    [HttpPost("available/email")]
    public async Task<IActionResult> EmailAvailabilityVerifier([FromBody] string email) {
        string response = await authService.CheckIfUsernameOrEmailExistsAsync(email);
        if (response.Equals(string.Empty)) {
            return BadRequest("An account with this email already exists!");
        }
    
        return NoContent();
    }
}