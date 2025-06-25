namespace Backend.DTOs;

public class CreateInterviewDto {
    public int SeekerId { get; set; }
    public int HirerId { get; set; }
    public int JobId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Mode { get; set; }
}