using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users;

public enum Roles {
    Hirer,
    Seeker,
    General
}

public class UserCredentials {
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public string Email { get; set; }
    public Roles Role { get; set; }
    
    [ForeignKey("Id")]
    public Guid UserId { get; set; }
}