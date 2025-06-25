using Backend.Models.Users;

namespace Backend.DTOs.Users;

public class SeekerProfileDto {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }
    
    public Address Address { get; set; }
    public string GithubUsername { get; set; }
    public int WorkExperienceInYears { get; set; }  
    public string ResumeURL { get; set; }
}