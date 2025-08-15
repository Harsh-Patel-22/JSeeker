using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Users;
using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AuthRepository (ApplicationDbContext context) {
    private readonly PasswordHasher<UserCredentials> passwordHasher = new PasswordHasher<UserCredentials>();
    public async Task<bool> RegisterUserCredentialsAsync(UserPrimaryDetailsFillUpDto credentials) {
        Guid userId = Guid.NewGuid();
        
        try {
            await context.UserCredentials.AddAsync(new UserCredentials() {
                Username = credentials.Username,
                Email = credentials.Email,
                HashedPassword = passwordHasher.HashPassword(new UserCredentials() ,credentials.Password),
                UserId = userId,
                Role = credentials.Role
            });
            
            // Adding Placeholder Data
            if (credentials.Role == Roles.Hirer) {
                await context.Hirers.AddAsync(new Hirer() {
                    Id = userId,
                    CompanyName = "",
                    Designation = "",
                    WebsiteLink = "",
                    CompanyAddressId = 1
                    
                });
            }

            await context.Users.AddAsync(new User() {
                Id = userId,
                FirstName = credentials.FirstName,
                LastName = credentials.LastName,
                // Occupation = "",
                Gender = Gender.PreferNotToSay,
                AddressId = 1, 
                
                AboutLine = "",
                Description = "",
                IsHirer = credentials.Role == Roles.Hirer,
                PhoneNumber = credentials.PhoneNumber,
                GithubUsername = "",
                LinkedInProfileLink = "https://linkedin.com/",
                ResumeJsonString = "",
                ResumeTemplateNumber = 0,
                
                NumberOfRejections = 0,
                NumberOfSuccessfulEmployments = 0,
                TechnicalKeywords = "",
                AIGeneratedTechnicalKeywords = "",
                Email = credentials.Email,
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