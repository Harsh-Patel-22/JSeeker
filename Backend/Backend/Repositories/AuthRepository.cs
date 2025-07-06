using Backend.Data;
using Backend.DTOs;
using Backend.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AuthRepository (ApplicationDbContext context) {
    private readonly PasswordHasher<UserCredentials> passwordHasher = new PasswordHasher<UserCredentials>();
    public async Task<bool> RegisterUserCredentialsAsync(RegisterCredentialsDto credentials) {
        try {
            await context.UserCredentials.AddAsync(new UserCredentials() {
                Id = context.UserCredentials.Count(),
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

    public bool LoginUser(LoginCredentialsDto credentials) {
        return (from dbCreds in context.UserCredentials
            where dbCreds.Username == credentials.Username || dbCreds.Email == credentials.Username
            select dbCreds).FirstOrDefault() != null;
    }

    public async Task<bool> CheckIfUsernameExistAsync(string username) {
        return await context.UserCredentials.Select(u => u.Username.Equals(username)).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckIfEmailExistAsync(string email) {
        return await  context.UserCredentials.Select(u => u.Email.Equals(email)).FirstOrDefaultAsync();
    }
}