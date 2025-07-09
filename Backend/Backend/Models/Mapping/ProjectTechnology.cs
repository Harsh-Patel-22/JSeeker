using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Users;
using Backend.Models.Users.WorkRelated;

namespace Backend.Models.Mapping;

public class ProjectTechnology
{
    [Key]
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int TechnologyId { get; set; }

    public float PercentUsage { get; set; }
    
    [ForeignKey("ProjectId")]
    public Project Project { get; set; }
    [ForeignKey("TechnologyId")]
    public Technology Technology { get; set; }
}
