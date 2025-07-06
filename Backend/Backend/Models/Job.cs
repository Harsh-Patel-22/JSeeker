using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Job {
    public Job(int id, string title, string description, string termsAndConditions, decimal salary, string status, int locationId) {
        Id = id;
        Title = title;
        Description = description;
        TermsAndConditions = termsAndConditions;
        Salary = salary;
        Status = status;
        LocationId = locationId;
    }

    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string TermsAndConditions { get; set; }
    public decimal Salary { get; set; }
    public string Status { get; set; }
    public int LocationId { get; set; }
    
    // Navigation Property
    [ForeignKey("LocationId")]
    public Location Location { get; set; }
}