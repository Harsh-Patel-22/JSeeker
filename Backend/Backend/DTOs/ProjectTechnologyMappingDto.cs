using Backend.Models.Users.WorkRelated;

namespace Backend.DTOs;

public record ProjectTechnologyMappingDto(
        Project Project,
        Dictionary<string, float> MappingData
    );