using Backend.DTOs.Job;

namespace Backend.DTOs.Users.Hirer;

public class HirerDashboardMetricsDto {
    public MetricsDto Metrics { get; set; }
    public List<InterviewBasicDetailsDto> ScheduledInterviews { get; set; }
}