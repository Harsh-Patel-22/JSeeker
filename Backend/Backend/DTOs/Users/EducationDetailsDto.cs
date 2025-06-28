namespace Backend.DTOs.Users;

public record EducationDetailsDto(
    string Name,
    string UniversityName,
    string State,
    string Country,
    DateOnly StartDate,
    DateOnly EndDate
    );