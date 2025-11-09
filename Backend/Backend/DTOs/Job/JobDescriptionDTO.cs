using Backend.Models;
using Backend.Models.Users;

namespace Backend.DTOs.Job;

public class JobDescriptionDto {
    public int Id { get; set; }
    public string Title { get; set; }
    public string CompanyName { get; set; }
    
    
    public string Description { get; set; }
    public string TermsAndConditions { get; set; }
    public string Responsibilities { get; set; }
    public int RequiredWorkExperience { get; set; }
    
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    
    // Post Details
    public JobStatus Status { get; set; }
    public WorkMode WorkMode { get; set; }
    public JobType Type { get; set; }
    
    public DateOnly PostDate { get; set; }
    public int NumberOfApplicants { get; set; }
    public int ApplicationsLimit { get; set; }
    
    public Address Address { get; set; }
    public Guid HirerId { get; set; }
    
    
}