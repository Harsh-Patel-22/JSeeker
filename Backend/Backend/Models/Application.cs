using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Users;

namespace Backend.Models;

public enum State {
    Open,
    Shortlisted,
    Scheduling,
    Scheduled,
    Rescheduling,
    Closed,
    Rejected
}

public class Application {
    [Key]
    public int Id { get; set; }
    public Guid ApplicantId { get; set; }
    public int JobId { get; set; }
    public Guid HirerId { get; set; }
    public State ApplicationState { get; set; }

    // Navigation Properties
    [ForeignKey("ApplicantId")]
    public User Applicant { get; set; }
    [ForeignKey("JobId")]
    public Job Job { get; set; }
    [ForeignKey("HirerId")]
    public User Hirer { get; set; }
}