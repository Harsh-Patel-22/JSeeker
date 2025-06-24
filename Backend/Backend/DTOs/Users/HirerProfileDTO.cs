using Backend.Models.Users;

namespace Backend.DTOs.Users;

public class HirerProfileDTO {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }
    public Address CompanyAddress { get; set; }
    
    //public int PendingApplications { get; set; }
    //public int PendingInterviews { get; set; }
    //no of applications job wise
}