using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users;

public class Education {
    [Key]
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Study { get; set; }
    public string InstituteName { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
}