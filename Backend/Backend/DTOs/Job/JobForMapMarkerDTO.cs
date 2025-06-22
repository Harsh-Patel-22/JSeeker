using Backend.Models;

namespace Backend.DTOs.Job;

public class JobForMapMarkerDTO {
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Distance { get; set; }
    public Location Location { get; set; }
}