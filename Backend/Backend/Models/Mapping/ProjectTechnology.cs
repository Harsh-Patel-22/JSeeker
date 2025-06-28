using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Mapping;

public class ProjectTechnology
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Id")]
    public int ProjectId { get; set; }
    [ForeignKey("Id")]
    public int TechnologyId { get; set; }

    public float PercentUsage { get; set; }
}