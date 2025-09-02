using System.Security.Claims;
using Backend.Exceptions;
using Backend.Models.Users;

namespace Backend.Extensions;

public static class ClaimsPrincipalExtension {
    public static Guid GetNameIdentifier(this ClaimsPrincipal claimsPrincipal) {
        string? idStr = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out Guid id)) {
            throw new GlobalExceptions.Unauthorised();
        }
        return id;
    }

    public static Roles GetRole(this ClaimsPrincipal claimsPrincipal) {
        string? rolesStr = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        if (string.IsNullOrEmpty(rolesStr) || !Enum.TryParse<Roles>(rolesStr, out Roles role)) {
            throw new GlobalExceptions.Unauthorised();
        }
        return role;
    }
}