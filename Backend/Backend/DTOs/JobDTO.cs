using Backend.Models;

namespace Backend.DTOs;

public class JobDto {
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string TermsAndConditions { get; set; }
    public decimal Salary { get; set; }
    public required Location Location { get; set; }
}