using Backend.Models;

namespace Backend.DTOs.Job;

public class JobCardDTO {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public string WorkMode { get; set; }
    public string Shift { get; set; }
    public string CompanyName { get; set; }
    public Location Location { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
}