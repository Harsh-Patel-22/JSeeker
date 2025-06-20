using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Interview {
    [Key]
    public int InterviewId { get; set; }
    [ForeignKey("")]
    public int SeekerId { get; set; }
    [ForeignKey("")]
    public int HirerId { get; set; }
    [ForeignKey("")]
    public int JobId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Mode { get; set; }
}