using Backend.Models.Users;

namespace Backend.DTOs.Users;

public class HirerProfileDetailsDto (
    string FirstName,
    string LastName ,
    string PhoneNumber ,
    Gender Gender ,
    Address CompanyAddress,
    
    string CompanyName,
    string Designation,
    string WebsiteLink
);