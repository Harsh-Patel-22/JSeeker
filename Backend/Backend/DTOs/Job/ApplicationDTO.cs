using Backend.Models;

namespace Backend.DTOs.Job;

public class ApplicationDto {
    public int ApplicationId { get; set; }
    public Guid ApplicantId { get; set; }
    public int JobId { get; set; }
    public Guid HirerId { get; set; }
    public Guid SeekerId { get; set; }
    
    
    public string JobTitle { get; set; }
    
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public ApplicationState State { get; set; }
    
    
    // TODO - Find a way to fetch this and send... 
    public List<string> Technologies { get; set; }
    public DateOnly AppliedOn { get; set; }
    public int AIGivenRating { get; set; }
    // public byte[]? PrecreatedResume { get; set; }
    public string? ResumeJsonString { get; set; }
    public int? ResumeTemplateNumber { get; set; }
}