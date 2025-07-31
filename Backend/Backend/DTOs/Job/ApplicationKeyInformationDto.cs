namespace Backend.DTOs.Job;

public record ApplicationKeyInformationDto (
    int JobId,
    Guid SeekerId,
    Guid HirerId
);