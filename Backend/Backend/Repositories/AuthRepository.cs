using Backend.Data;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AuthRepository (ApplicationDbContext context) {
    private readonly PasswordHasher<UserCredentials> passwordHasher = new PasswordHasher<UserCredentials>();
    public async Task<bool> RegisterUserCredentialsAsync(RegisterCredentialsDto credentials) {
        Guid userId = Guid.NewGuid();
        Enum.TryParse(credentials.Role, out Roles role);
        
        try {
            await context.UserCredentials.AddAsync(new UserCredentials() {
                Username = credentials.Username,
                Email = credentials.Email,
                HashedPassword = passwordHasher.HashPassword(new UserCredentials() ,credentials.Password),
                UserId = userId,
                Role = role
            });
            
            // Adding Placeholder Data
            if (role == Roles.Hirer) {
                await context.Hirers.AddAsync(new Hirer() {
                    Id = userId,
                    CompanyName = ".",
                    Designation = ".",
                    WebsiteLink = "www....",
                    CompanyAddressId = 1
                    
                });
            }

            await context.Users.AddAsync(new User() {
                Id = userId,
                FirstName = ".",
                LastName = ".",
                Occupation = ".",
                Gender = Gender.PreferNotToSay,
                AddressId = 1, 
                
                AboutLine = ".",
                Description = ",",
                
                PhoneNumber = ".",
                GithubUsername = ".",
                LinkedInProfileLink = "https://linkedin.com/",
                ResumeJsonString = "empty",
                
                JobPreference = JobType.Internship
                
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

        var hashedPassword = await context.UserCredentials.Where(u => u.Username == credentials.Username || u.Email == credentials.Username).Select(u => u.HashedPassword).FirstOrDefaultAsync();
        if (hashedPassword == null) {
            return false;
        }
        
        return passwordHasher.VerifyHashedPassword(new UserCredentials(), hashedPassword, credentials.Password) != PasswordVerificationResult.Failed;
    }

    public async Task<bool> CheckIfUsernameExistAsync(string username) {
        return await context.UserCredentials.Where(u => u.Username.Equals(username)).FirstOrDefaultAsync() != null;
    }

    public async Task<bool> CheckIfEmailExistAsync(string email) {
        return await  context.UserCredentials.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync() != null;
    }

    public async Task<Guid> GetUserIdAsync(IJwtUser credentials) {
        return await context.UserCredentials.Where(uc => uc.Username == credentials.Username).Select(u => u.UserId).FirstOrDefaultAsync();
    }
}