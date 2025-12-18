namespace Backend.DTOs.Users.Hirer;

public class MetricsDto {
    public int NumNewApplicationsToday { get; set; }
    public int NumActiveJobOpenings {get; set;}
    public int TotalHires {get; set;}
    public decimal HiringRate {get; set;}
}