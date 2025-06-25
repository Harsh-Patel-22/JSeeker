using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models;

namespace Backend.DTOs;

public class CreateJobDto {
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string TermsAndConditions { get; set; }
    public decimal Salary { get; set; }
    public string Status { get; set; }
    [ForeignKey("Id")]
    public int LocationId { get; set; }
    public Location Location { get; set; }
    
}