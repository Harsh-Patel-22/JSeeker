using System.Text.Json;
using Backend.DTOs.Users;

namespace Backend.DTOs;

public class ResumeContentsDto {
    public BasicDetailsDto? BasicDetails {get; set;}
    public ContactDetailsDto? ContactDetails {get; set;}
    public  Dictionary<string, ProjectDetailsDto> ProjectDetails {get; set;}
    public List<WorkExperienceDetailsDto> WorkExperienceDetails {get; set;} 
    public List<EducationDetailsDto> EducationDetails {get; set;}
    public List<LanguageDto> LanguageDetails {get; set;}
}