namespace Backend.DTOs.Users;

public record WorkExperienceDetailsDto (
        string Role,
        string Description,
        string CompanyName,
        DateOnly StartDate,
        DateOnly EndDate
);