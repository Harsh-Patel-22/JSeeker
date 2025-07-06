using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users.WorkRelated;

public class WorkExperience {
    [Key]
    public int Id { get; set; }
    [ForeignKey("Id")]
    public Guid UserId { get; set; }
    public string Role { get; set; }
    public string Description { get; set; }
    public string InstituteName { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    
    // Navigation Property
    [ForeignKey("UserId")]
    public User User { get; set; }
}