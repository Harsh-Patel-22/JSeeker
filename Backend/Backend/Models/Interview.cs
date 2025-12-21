using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Users;

namespace Backend.Models;

public enum InterviewMode {
    Online,
    InPerson
}

public enum InterviewState {
    Scheduled,
    Taken,
    Updates
}

public enum InterviewOutcome {
    Pending,
    Hired,
    Rejected,
    DidntAppear
}

public class Interview {
    [Key]
    public int ApplicationId { get; set; }
    public Guid SeekerId { get; set; }
    public Guid HirerId { get; set; }
    public int JobId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public InterviewMode Mode { get; set; }
    public bool ConfirmedByHirer { get; set; }
    public bool ConfirmedBySeeker { get; set; }
    
    public InterviewOutcome Outcome { get; set; }
    // Navigation Properties
    [ForeignKey("HirerId")]
    public User Hirer { get; set; }
    
    [ForeignKey("SeekerId")]
    public User Seeker { get; set; }
    
    [ForeignKey("JobId")]
    public Job Job { get; set; }
    [ForeignKey("ApplicationId")]
    public Application Application { get; set; }
}