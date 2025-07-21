using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Users;

namespace Backend.Models;

// TODO - Maybe I don't need the JobStatus Field
public enum JobStatus {
    Open,
    ClosingSoon,
    Closed,
}

public enum JobType {
    Internship,
    Job
}

public enum WorkMode {
    OnSite,
    WorkFromHome
}

public class Job {
    
    
    // Basic
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string TermsAndConditions { get; set; }
    public string Responsibilities { get; set; }
    
    [Range(0, 10)]
    public int RequiredWorkExperience { get; set; }
    public string CompanyName { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    
    
    // Post Details
    public JobStatus Status { get; set; }
    public JobType Type { get; set; }
    public WorkMode WorkMode { get; set; }
    public int ApplicationsLimit { get; set; }
    public DateOnly PostDate { get; set; }
    public int NumberOfApplications { get; set; }
    
    
    // ForeignKey References
    public int AddressId { get; set; }
    public Guid HirerId { get; set; }
    
    
    // Navigation Property
    [ForeignKey("AddressId")]
    public Address Address { get; set; }
    [ForeignKey("HirerId")]
    public Hirer Hirer { get; set; }
}