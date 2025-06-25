using Backend.Models;

namespace Backend.DTOs.Users;

public record BasicDetailsDto(
    string FirstName,
    string LastName,
    string Occupation,
    string State,
    string Country,
    string AboutLine
    );