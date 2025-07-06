using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Mapping;

public class UserHobby
{
    // TODO - Not really needed
    [Key]
    public int Id { get; set; }
    [ForeignKey("Id")]
    public int HobbyId { get; set; }
    [ForeignKey("Id")]
    public Guid UserId { get; set; }
}