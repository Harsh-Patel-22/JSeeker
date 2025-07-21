using Backend.Models;
using Backend.Models.Users;

namespace Backend.DTOs;

public class JobDto {
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string TermsAndConditions { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public required Address Address { get; set; }
}