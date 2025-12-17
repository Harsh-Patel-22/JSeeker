using Backend.Models;

namespace Backend.DTOs.Job;

public class InterviewBasicDetailsDto {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public InterviewMode Mode { get; set; }
}