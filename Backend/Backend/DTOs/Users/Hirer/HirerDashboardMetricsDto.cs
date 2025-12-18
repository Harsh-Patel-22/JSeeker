using Backend.DTOs.Job;

namespace Backend.DTOs.Users.Hirer;

public class HirerDashboardMetricsDto {
    public MetricsDto MetricsDto { get; set; }
    public List<InterviewBasicDetailsDto> ScheduledInterviews { get; set; }
}