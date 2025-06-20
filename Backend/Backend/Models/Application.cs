using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Application {
    [Key]
    public int ApplicationId { get; set; }
    [ForeignKey("")]
    public int ApplicantId { get; set; }
    [ForeignKey("")]
    public int JobId { get; set; }
    [ForeignKey("")]
    public int HirerId { get; set; }
}