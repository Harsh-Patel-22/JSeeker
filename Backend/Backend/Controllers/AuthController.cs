using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DTOs;
using Backend.Models.Users;
using Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase {
    private readonly IPasswordHasher<UserCredentials> _passwordHasher = new PasswordHasher<UserCredentials>();

    [HttpPost("login")]
    // ReSharper disable once InconsistentNaming
    public IActionResult VerifyLoginCredentials([FromBody] LoginCredentialsDto credentials) {
        string jwtToken = authService.LoginUser(credentials);
        if(jwtToken.Equals(string.Empty)) {
            return BadRequest();
        }
        return Ok(jwtToken);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterNewUser([FromBody] RegisterCredentialsDto credentials) {
        string jwtToken = await authService.RegisterNewUser(credentials);
        if (jwtToken.Equals(string.Empty)) {
            return BadRequest("User already registered!");
        }
        return Ok(jwtToken);
    }
    
    // Async Calls from frontend - Checker Functions
    [HttpPost("available/username")]
    public async Task<IActionResult> UsernameAvailabilityVerifier([FromBody] string username) {
        string response = await authService.CheckIfUsernameOrEmailExists(username);
        if (response.Equals(string.Empty)) {
            return BadRequest("Username already taken!");
        }
        return NoContent();
    }
    
    [HttpPost("available/email")]
    public async Task<IActionResult> EmailAvailabilityVerifier([FromBody] string email) {
        string response = await authService.CheckIfUsernameOrEmailExists(email);
        if (response.Equals(string.Empty)) {
            return BadRequest("An account with this email already exists!");
        }
    
        return NoContent();
    }
}