namespace Backend.DTOs;

public record MetricsDto(
    int TotalUsers,
    int TotalJobsPosted,
    float AverageJobsPostedDaily,
    int NumberOfSuccessfulJobLandings,
    float JobLandingSuccessRate
    );