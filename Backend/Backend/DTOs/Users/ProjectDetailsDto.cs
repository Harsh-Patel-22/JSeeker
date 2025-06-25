namespace Backend.DTOs.Users;

public record ProjectDetailsDto(
    string Name,
    // TODO - Add technologies list
    DateOnly StartDate,
    DateOnly EndDate,
    string GithubRepoLink
);