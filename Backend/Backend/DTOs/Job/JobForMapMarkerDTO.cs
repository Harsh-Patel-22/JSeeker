using Backend.Models;
using Backend.Models.Users;

namespace Backend.DTOs.Job;

public class JobForMapMarkerDto {
    public int Id { get; set; }
    public string Title { get; set; }
    public string CompanyName { get; set; }
    public decimal Distance { get; set; }
    public Address Address { get; set; }
}