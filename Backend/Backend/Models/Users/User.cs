using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users;

// TODO - Add all basic deatils here and add a work related section for the hirer. So basic all deatils would be there for either login. Hirer will have an add on to view the statuses of the work related thing
public enum Gender {
    Male,
    Female,
    Others,
    PreferNotToSay
}

public class User {
    
    // Section - Personal Details
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Occupation { get; set; }
    public Gender Gender { get; set; }
    public int AddressId { get; set; }
    
    // Section - Know more
    public string AboutLine { get; set; }
    public string Description { get; set; }
    
    // Section - Contact Details
    public string PhoneNumber { get; set; }
    public string GithubProfileLink { get; set; }
    public string LinkedInProfileLink { get; set; }
    
    // TODO - In the ui, while creating new post, if the mode in on site, give 2 options, company location and add new location. Also, only ask for location if its on site, else no need to show.
    [ForeignKey("Id")]
    public bool IsSeeker { get; set; }
}