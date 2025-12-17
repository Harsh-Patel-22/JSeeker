using Backend.Models.Users.Cocurricular;

namespace Backend.DTOs.Users;

public class UserProfileDetailsDto {
    public BasicDetailsDto? BasicDetails { get; set; }
    public List<EducationDetailsDto> EducationDetails { get; set; }
    public List<WorkExperienceDetailsDto> WorkExperienceDetails { get; set; }
    public List<ProjectDetailsDto> ProjectDetails { get; set; }
    public List<LanguageDto> VocalLanguage { get; set; }
    public ContactDetailsDto? ContactDetails { get; set; }
}