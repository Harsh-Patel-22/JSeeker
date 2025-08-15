using Backend.Models;
using Backend.Models.Users;

namespace Backend.DTOs.Users;

public class UserSecondaryDetailsDto {
    public Address Address {get; set;}
    public Gender Gender {get; set;}
    public string AboutLine {get; set;}
    public string Description {get; set;}
    public string LinkedInProfileLink {get; set;}

    public List<EducationDetailsDto> EducationDetails {get; set;}
    public List<WorkExperienceDetailsDto> WorkExperienceDetails {get; set;}
    public List<LanguageDto> VocalLanguageDetails {get; set;}
    
    public JobType JobPreference {get; set;}
    
    
}