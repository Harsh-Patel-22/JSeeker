using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users.WorkRelated;

public class Project
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    
    [ForeignKey("Id")]
    public int UserId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string GithubRepoLink { get; set; }
    
}