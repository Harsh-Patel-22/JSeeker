using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Users;

public class Hirer {
    [Key]
    public int Id { get; set; }
    [ForeignKey("id")]
    public int CompanyAddressId { get; set; }

    public string CompanyName { get; set; }
    public string Designation { get; set; }
    public string WebsiteLink { get; set; }
}