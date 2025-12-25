using Backend.Models.Users;

namespace Backend.DTOs.Users;

public class HirerProfileDetailsDto {
    
    public string FirstName {get; set; }
    public string LastName  {get; set; }
    public string PhoneNumber  {get; set; }
    public Gender Gender  {get; set; }
    public Address CompanyAddress {get; set; }

    public string CompanyName {get; set; }
    public string Designation {get; set; }
    public string WebsiteLink {get; set; }
}
