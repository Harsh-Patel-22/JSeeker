using Backend.Interfaces;
using Backend.Models.Users;

namespace Backend.DTOs;

public record LoginCredentialsDto (
    string Username,
    string Password,
    Roles Role
) : IJwtUser;