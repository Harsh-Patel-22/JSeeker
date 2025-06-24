using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users;

public class UserCredentials {
    [Key]
    public int Id { get; set; }
    public HashSet<string> Username { get; set; }
    public HashSet<string> Password { get; set; }
    
    [ForeignKey("Id")]
    public int UserId { get; set; }
}