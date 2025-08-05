using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Mapping;

namespace Backend.Models.Users.WorkRelated;

public class Project
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Guid UserId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly LastUpdatedDate { get; set; }
    
    public bool IsCompleted { get; set; }
    public string GithubRepoLink { get; set; }
    
    
    // Navigation Property
    [ForeignKey("UserId")]
    public User User { get; set; }
    public ICollection<ProjectTechnology> ProjectTechnologies { get; set; }
}