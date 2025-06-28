using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Users.WorkRelated;

public class Technology
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}