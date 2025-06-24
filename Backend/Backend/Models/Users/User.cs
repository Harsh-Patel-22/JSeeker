namespace Backend.Models.Users;


public enum Gender {
    Male,
    Female,
    Others,
    PreferNotToSay
}

public enum Role {
    Hirer,
    Seeker
}

public class User {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }
    
    // TODO - In the ui, while creating new post, if the mode in on site, give 2 options, company location and add new location. Also, only ask for location if its on site, else no need to show.
    
    public Role Role { get; set; }
}