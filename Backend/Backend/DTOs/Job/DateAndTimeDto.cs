namespace Backend.DTOs.Job;

public record DateAndTimeDto(
    DateOnly Date,
    TimeOnly Time
    );