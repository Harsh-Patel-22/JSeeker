using Backend.Models;
using Backend.Models.Users;

namespace Backend.DTOs.Job;

public class JobCardDto {
    public int Id { get; set; }
    public string Title { get; set; }
    public JobType Type { get; set; }
    public JobStatus Status { get; set; }
    public WorkMode WorkMode { get; set; }
    // public string Shift { get; set; }
    public string CompanyName { get; set; }
    public Address Address { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
}