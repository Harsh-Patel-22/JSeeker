using Backend.Models;

namespace Backend.DTOs;

public class CreateInterviewDto {
    public int ApplicationId { get; set; }
    public Guid SeekerId { get; set; }
    public Guid HirerId { get; set; }
    public int JobId { get; set; }
    public DateOnly DateProposedByHirer { get; set; }
    public TimeOnly TimeProposedByHirer { get; set; }
    public InterviewMode Mode { get; set; }
}