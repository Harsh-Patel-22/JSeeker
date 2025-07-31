using Backend.Models;

namespace Backend.DTOs.Job;

public record ApplicationStateUpdateDto (
    int ApplicationId,
    ApplicationState State
    );