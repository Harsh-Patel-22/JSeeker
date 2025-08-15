using Backend.Interfaces;
using Backend.Models.Users;

namespace Backend.DTOs.Users;

public class UserPrimaryDetailsFillUpDto : IJwtUser {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    
    public string Username { get; set; }
    public string Password { get; set; }
    
    public Roles Role { get; set; }
}