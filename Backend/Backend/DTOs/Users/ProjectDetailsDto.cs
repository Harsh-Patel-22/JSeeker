namespace Backend.DTOs.Users;

public record ProjectDetailsDto(
    int Id,
    string Name,
    string Description,
    List<TechnologyUsageDto> TechnologiesUsages, 
    DateOnly StartDate,
    DateOnly LastUpdatedDate,
    string GithubRepoLink
);