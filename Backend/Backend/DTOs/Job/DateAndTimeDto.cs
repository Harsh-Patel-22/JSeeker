namespace Backend.DTOs;

public record DateAndTimeDto(
    DateOnly Date,
    TimeOnly Time
    );