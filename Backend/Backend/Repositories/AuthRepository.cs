using Backend.Data;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AuthRepository (ApplicationDbContext context) {
    private readonly PasswordHasher<UserCredentials> passwordHasher = new PasswordHasher<UserCredentials>();
    public async Task<bool> RegisterUserCredentialsAsync(RegisterCredentialsDto credentials) {
        try {
            await context.UserCredentials.AddAsync(new UserCredentials() {
                Username = credentials.Username,
                Email = credentials.Email,
                HashedPassword = passwordHasher.HashPassword(new UserCredentials() ,credentials.Password),
                UserId = Guid.NewGuid()
            });

            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e) {
            return false;
        }
    }

    public async Task<bool> LoginUserAsync(LoginCredentialsDto credentials) {
        var dbCreds = context.UserCredentials.Where(u => u.Username == credentials.Username || u.Email == credentials.Username);
        if (await dbCreds.FirstOrDefaultAsync() == null) {
            return false;
        }

        var hashedPassword = await context.UserCredentials.Where(u => u.Username == credentials.Username).Select(u => u.HashedPassword).FirstOrDefaultAsync();
        if (hashedPassword == null) {
            return false;
        }
        
        return passwordHasher.VerifyHashedPassword(new UserCredentials(), hashedPassword, credentials.Password) != PasswordVerificationResult.Failed;
    }

    public async Task<bool> CheckIfUsernameExistAsync(string username) {
        return await context.UserCredentials.Select(u => u.Username.Equals(username)).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckIfEmailExistAsync(string email) {
        return await  context.UserCredentials.Select(u => u.Email.Equals(email)).FirstOrDefaultAsync();
    }

    public async Task<Guid> GetUserIdAsync(IJwtUser credentials) {
        return await context.UserCredentials.Select(u => u.UserId).FirstOrDefaultAsync();
    }
}