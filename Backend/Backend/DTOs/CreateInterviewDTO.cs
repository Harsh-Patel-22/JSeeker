namespace Backend.DTOs;

public class CreateInterviewDto {
    public Guid SeekerId { get; set; }
    public Guid HirerId { get; set; }
    public int JobId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Mode { get; set; }
}