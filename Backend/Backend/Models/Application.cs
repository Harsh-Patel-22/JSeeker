using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Users;

namespace Backend.Models;

public enum ApplicationState {
    // Default
    Pending,
    // Hirer can set
    Shortlisted,
    Rejected,
    
    // Set via the program
    InterviewScheduling,
    InterviewScheduled,
    InterviewRescheduling
}

public class Application {
    [Key]
    public int Id { get; set; }
    public Guid SeekerId { get; set; }
    public int JobId { get; set; }
    public Guid HirerId { get; set; }
    public ApplicationState State { get; set; }
    [Range(1, 10)]
    public int AIGivenRating { get; set; }
    
    public byte[]? PreCreatedResume { get; set; }
    
    public DateOnly AppliedOn { get; set; }

    // Navigation Properties
    [ForeignKey("SeekerId")]
    public User Seeker { get; set; }
    [ForeignKey("JobId")]
    public Job Job { get; set; }
    [ForeignKey("HirerId")]
    public User Hirer { get; set; }
}