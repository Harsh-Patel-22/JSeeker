using Backend.Models;

namespace Backend.DTOs.Job;

public class JobDescriptionDTO {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public string WorkMode { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public Location Location { get; set; }
    
    
    public string Description { get; set; }
    public string TermsAndConditions { get; set; }
    public string Requirements { get; set; }
    public string Miscellaneous { get; set; }
    
    
    public DateOnly PostedDaysAgo { get; set; }
    public int NumberOfApplicants { get; set; }
    
}