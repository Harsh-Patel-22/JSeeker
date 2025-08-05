using System.Security.Claims;
using Backend.Exceptions;

namespace Backend.Extensions;

public static class ClaimsPrincipalExtension {
    public static Guid GetNameIdentifier(this ClaimsPrincipal claimsPrincipal) {
        string? idStr = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(idStr) || !Guid.TryParse(idStr, out Guid id)) {
            throw new GlobalExceptions.Unauthorised();
        }
        return id;
    }
}