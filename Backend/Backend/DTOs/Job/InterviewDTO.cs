using Backend.Models;

namespace Backend.DTOs.Job;

public class InterviewDto {
    public int Id {get; set;}
    public int JobId { get; set; }
    public Guid SeekerId { get; set; }
    public Guid HirerId { get; set; }
    
    // Fields related to the seeker
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    // TODO - Think if possible to create in an easy and efficient manner
    // public List<string> GithubProjectLinks { get; set; }
    
    // Fields related to the job
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string JobDescription { get; set; }
    public string JobTermsAndConditions { get; set; }
    public string JobResponsibilities { get; set; }
    
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public InterviewMode Mode { get; set; }
    public bool ConfirmedByHirer { get; set; }
    public bool ConfirmedBySeeker { get; set; }

    public InterviewOutcome Outcome { get; set; }
}