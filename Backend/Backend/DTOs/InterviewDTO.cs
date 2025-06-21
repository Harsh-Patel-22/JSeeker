namespace Backend.DTOs;

public class InterviewDTO {
    public int SeekerId { get; set; }
    public int HirerId { get; set; }
    // TODO - remove the JobId. Not needed
    public int JobId { get; set; }
    public JobDTO Job { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Mode { get; set; }
}