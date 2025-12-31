namespace Backend.DTOs.Users;

public record EducationDetailsDto(
    int Id,
    string Study,
    string InstituteName,
    string State,
    string Country,
    DateOnly StartDate,
    DateOnly EndDate
    );