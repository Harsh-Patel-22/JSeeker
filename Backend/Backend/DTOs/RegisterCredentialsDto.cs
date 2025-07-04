using Backend.Interfaces;

namespace Backend.DTOs;

public record RegisterCredentialsDto(
    string Username,
    string Password,
    string Email,
    string Role
    ) : IJwtUser;