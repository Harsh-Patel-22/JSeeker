using Backend.Models.Users;

namespace Backend.DTOs.Users;

public record HirerProfileDto (
    string FirstName,
    string LastName ,
    string PhoneNumber ,
    Gender Gender ,
    Address CompanyAddress
    
    //public int PendingApplications ,
    //public int PendingInterviews ,
    //no of applications job wise
);