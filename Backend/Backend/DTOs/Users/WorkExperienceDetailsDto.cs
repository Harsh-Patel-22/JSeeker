namespace Backend.DTOs.Users;

public record WorkExperienceDetailsDto (
        int Id,
        string Role,
        string Description,
        string CompanyName,
        DateOnly StartDate,
        DateOnly EndDate
);