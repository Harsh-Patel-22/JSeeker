namespace Backend.DTOs.Users;

public record WorkExperienceDetailsDto (
        string Role,
        string Description,
        string State,
        string Country,
        DateOnly StartDate,
        DateOnly EndDate
);