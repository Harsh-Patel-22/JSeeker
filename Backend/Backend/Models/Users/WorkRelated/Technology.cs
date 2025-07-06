using System.ComponentModel.DataAnnotations;
using Backend.Models.Mapping;

namespace Backend.Models.Users.WorkRelated;

public class Technology
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<ProjectTechnology> ProjectTechnologyies { get; set; }
}