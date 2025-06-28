using Backend.Models.Mapping;

namespace Backend.DTOs.Users;

public record LanguageDto(
    string Name,
    LanguageLevel Level
);