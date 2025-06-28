using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Users.Cocurricular;

public class Hobby
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
}