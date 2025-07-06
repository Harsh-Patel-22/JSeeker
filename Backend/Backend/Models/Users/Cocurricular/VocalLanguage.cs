using Backend.Models.Mapping;

namespace Backend.Models.Users.Cocurricular;

public class VocalLanguage
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<UserVocalLanguage> UserVocalLanguages { get; set; }
}