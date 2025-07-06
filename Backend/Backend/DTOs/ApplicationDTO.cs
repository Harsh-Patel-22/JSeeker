namespace Backend.DTOs;

public class ApplicationDto {
    public Guid ApplicantId { get; set; }
    public int JobId { get; set; }
    public Guid HirerId { get; set; }
}