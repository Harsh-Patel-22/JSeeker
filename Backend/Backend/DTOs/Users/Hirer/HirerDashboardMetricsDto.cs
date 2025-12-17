using Backend.DTOs.Job;

namespace Backend.DTOs.Users.Hirer;

public class HirerDashboardMetricsDto {
    public int NumNewApplicationsToday { get; set; }
    public int NumActiveJobOpenings {get; set;}
    public int TotalHires {get; set;}
    public decimal HiringRate {get; set;}
    public List<InterviewBasicDetailsDto> ScheduledInterviews { get; set; }
}