using Backend.Models.Users;

namespace Backend.DTOs;

public record CompanyDetailsDto(
    string Name,
    int Address
    );