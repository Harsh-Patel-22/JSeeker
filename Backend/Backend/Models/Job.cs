using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Job {
    [Key]
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string TermsAndConditions { get; set; }
    public decimal Salary { get; set; }
    public string Status { get; set; }
    [ForeignKey("Id")]
    public int LocationId { get; set; }
    public Location Location { get; set; }
}