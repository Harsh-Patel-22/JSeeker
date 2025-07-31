using Backend.Models.Users;

namespace Backend.DTOs.Users.Hirer;

public record HirerProfessionalDetailsDto(
     string CompanyName,
     string Designation,
     string WebsiteLink,
    
     Address CompanyAddress
    );