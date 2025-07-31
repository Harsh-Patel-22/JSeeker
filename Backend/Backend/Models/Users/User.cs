using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Mapping;
using Backend.Models.Users.Cocurricular;
using Backend.Models.Users.WorkRelated;

namespace Backend.Models.Users;

// TODO - Add all basic deatils here and add a work related section for the hirer. So basic all deatils would be there for either login. Hirer will have an add on to view the statuses of the work related thing
public enum Gender {
    Male,
    Female,
    Others,
    PreferNotToSay
}

public class User {
    // TODO - Find a way to properly populate the link between credentials class and this class
    
    // Section - Personal Details
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Occupation { get; set; }
    public Gender Gender { get; set; }
    public int? AddressId { get; set; }
    
    // Section - Know more
    public string AboutLine { get; set; }
    public string Description { get; set; }
    
    // Section - Contact Details
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string GithubUsername { get; set; }
    public string LinkedInProfileLink { get; set; }
    public string? ResumeJsonString { get; set; }
    public int? ResumeTemplateNumber { get; set; }
    public int NumberOfSuccessfulEmployments { get; set; }
    public int NumberOfRejections { get; set; }
    
    // TODO - Might wanna remove the below field...
    public byte[]? PreCreatedResume { get; set; } 
    
    // TODO - In the ui, while creating new post, if the mode in on site, give 2 options, company location and add new location. Also, only ask for location if its on site, else no need to show.
    public bool IsHirer { get; set; }
    
    // Section - Professional Details
    public ICollection<Project> Projects { get; set; }
    public ICollection<WorkExperience> WorkExperiences { get; set; }
    public ICollection<Education> Educations { get; set; }
    public JobType JobPreference {get; set;}
    
    // Section - Other Details
    public ICollection<Hobby> Hobbies { get; set; }
    public ICollection<UserVocalLanguage> UserVocalLanguages { get; set; }
    
    // Navigation Properties
    [ForeignKey("AddressId")]
    public Address Address { get; set; }
}