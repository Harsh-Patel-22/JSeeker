using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users;

public class Seeker {
    [Key]
    public int Id { get; set; }
    [ForeignKey("id")]
    public int AddressId { get; set; }
    public string GithubUsername { get; set; }
    public int WorkExperienceInYears { get; set; }  
    public string ResumeURL { get; set; }
    // TODO - To find a way to store the resume itself... 
    // education
    
}