using Backend.DTOs.Users;

namespace Backend.DTOs;

public class UpdatedResumeContentsDto {

    public BasicDetailsDto? BasicDetails {get; set;}
    public ContactDetailsDto? ContactDetails {get; set;}

    public List<ProjectDetailsDto> ProjectDetails {get; set;}
    public List<WorkExperienceDetailsDto> WorkExperienceDetails {get; set;} 
    public List<EducationDetailsDto> EducationDetails {get; set;}
    public List<LanguageDto> LanguageDetails {get; set;}
    
    public List<ProjectDetailsDto> DeletedProjectDetails {get; set;}
    public List<WorkExperienceDetailsDto> DeletedWorkExperienceDetails {get; set;} 
    public List<EducationDetailsDto> DeletedEducationDetails {get; set;}
   
}
