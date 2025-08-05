using Backend.Models;

namespace Backend.DTOs.Job;

public record JobSearchFilterDto(
    JobType type,
    JobStatus status,
    WorkMode mode
    // WorkMode[] mode
    );