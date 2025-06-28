using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users;

public class Education {
    [Key]
    public int Id { get; set; }
    [ForeignKey("Id")] 
    public int UserId { get; set; }
    public string Study { get; set; }
    public string InstituteName { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}