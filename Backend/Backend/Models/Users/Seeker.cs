using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users;

public class Seeker {
    [Key]
    public Guid Id { get; set; }

    public string GithubUsername { get; set; }
    public int WorkExperienceInYears { get; set; }  
    public string ResumeUrl { get; set; }
    // TODO - To find a way to store the resume itself... 
    // education
    
    
    
}