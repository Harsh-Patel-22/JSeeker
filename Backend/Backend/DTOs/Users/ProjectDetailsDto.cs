namespace Backend.DTOs.Users;

public record ProjectDetailsDto(
    string Name,
    List<TechnologyUsageDto> Technologies, 
    DateOnly StartDate,
    DateOnly EndDate,
    string GithubRepoLink
);