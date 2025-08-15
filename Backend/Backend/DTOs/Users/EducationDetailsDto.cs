namespace Backend.DTOs.Users;

public record EducationDetailsDto(
    string Study,
    string InstituteName,
    string State,
    string Country,
    DateOnly StartDate,
    DateOnly EndDate
    );