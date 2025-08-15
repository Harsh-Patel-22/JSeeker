using Backend.Models;

namespace Backend.DTOs.Users;

public record BasicDetailsDto(
    string FirstName,
    string LastName,
    string State,
    string Country,
    string AboutLine
    );