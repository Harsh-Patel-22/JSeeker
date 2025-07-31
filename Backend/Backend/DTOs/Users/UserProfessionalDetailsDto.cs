using Backend.Models.Users.WorkRelated;

namespace Backend.DTOs.Users;

public record UserProfessionalDetailsDto (
    List<ProjectDetailsDto> Projects,
    List<WorkExperienceDetailsDto> WorkExperience,
    List<EducationDetailsDto> Education
    );