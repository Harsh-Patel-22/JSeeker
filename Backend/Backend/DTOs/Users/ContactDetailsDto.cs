namespace Backend.DTOs.Users;

public record ContactDetailsDto(
    string Email,
    string GithubProfileLink,
    string LinkedInProfileLink,
    string PhoneNumber
    );