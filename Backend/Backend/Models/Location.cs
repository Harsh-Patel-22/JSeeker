using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Location {
    [Key]
    public int Id { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}