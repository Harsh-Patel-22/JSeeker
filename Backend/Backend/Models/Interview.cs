using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models.Users;

namespace Backend.Models;

public class Interview {
    [Key]
    public int Id { get; set; }
    public Guid SeekerId { get; set; }
    public Guid HirerId { get; set; }
    public int JobId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Mode { get; set; }
    
    
    // Navigation Properties
    [ForeignKey("HirerId")]
    public User Hirer { get; set; }
    
    [ForeignKey("SeekerId")]
    public User Seeker { get; set; }
    
    [ForeignKey("JobId")]
    public Job Job { get; set; }
}