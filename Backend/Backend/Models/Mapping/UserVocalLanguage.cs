using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Users;
using Backend.Models.Users.Cocurricular;

namespace Backend.Models.Mapping;

public enum LanguageLevel
{
    Fluent,
    Native,
    Learning
}

public class UserVocalLanguage
{
    [Key]
    public int Id { get; set; }
    public int VocalLanguageId { get; set; }
    public Guid UserId { get; set; }
    public LanguageLevel Level { get; set; }
    
    // Navigation Property
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    [ForeignKey("VocalLanguageId")]
    public VocalLanguage VocalLanguage { get; set; }
}