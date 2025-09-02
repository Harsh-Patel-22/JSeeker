using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models;
using Backend.Models.Users;

namespace Backend.DTOs;

public class CreateJobDto {
    public string Title { get; set; }
    public string Description { get; set; }
    public string TermsAndConditions { get; set; }
    public string Responsibilities { get; set; }
    
    public int RequiredWorkExperience { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    
    // Post Details
    public JobType Type { get; set; }
    public WorkMode WorkMode { get; set; }
    public int ApplicationsLimit { get; set; }
    
    
}