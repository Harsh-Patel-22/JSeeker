using Backend.Interfaces;
using Backend.Models.Users;

namespace Backend.DTOs;

public record RegisterCredentialsDto(
    string Username,
    string Password,
    string Email,
    Roles Role
    ) : IJwtUser;