using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [ForeignKey("Id")]
    public int VocalLanguageId { get; set; }
    [ForeignKey("Id")]
    public int UserId { get; set; }
    public LanguageLevel Level { get; set; }
}