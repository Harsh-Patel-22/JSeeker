using Backend.Models;

namespace Backend.DTOs.Job;

public record JobKeyInformationDto (
    // int JobId,
    JobStatus JobStatus,
    JobType JobType,
    int ApplicationsLimit
    );
