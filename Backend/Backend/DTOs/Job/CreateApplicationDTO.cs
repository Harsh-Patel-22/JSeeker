using Backend.Models;

namespace Backend.DTOs.Job;

public class CreateApplicationDto {
    public Guid SeekerId { get; set; }
    public int JobId { get; set; }
    public Guid HirerId { get; set; }
    public JobType JobType { get; set; }
    public int AIRating { get; set; }
}