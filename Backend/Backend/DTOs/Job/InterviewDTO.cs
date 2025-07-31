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
    
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public InterviewMode Mode { get; set; }
    public bool ConfirmedByHirer { get; set; }
    public bool ConfirmedBySeeker { get; set; }
}