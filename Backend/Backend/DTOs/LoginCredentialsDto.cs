using Backend.Interfaces;

namespace Backend.DTOs;

public record LoginCredentialsDto (
    string Username,
    string Password,
    string Role
) : IJwtUser;