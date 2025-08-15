using Backend.DTOs.Users;
using Backend.Models.Users.WorkRelated;

namespace Backend.DTOs;

public record ProjectTechnologyMappingDto(
        Project Project,
        List<TechnologyUsageDto> TechnologyUsages
    );