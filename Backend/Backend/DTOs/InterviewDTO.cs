namespace Backend.DTOs;

public class InterviewDto {
    public Guid SeekerId { get; set; }
    public Guid HirerId { get; set; }
    // TODO - remove the JobId. Not needed
    public int JobId { get; set; }
    public JobDto Job { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Mode { get; set; }
}