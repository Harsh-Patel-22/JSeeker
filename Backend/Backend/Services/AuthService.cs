using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models.Users;
using Backend.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services;

public class AuthService (IConfiguration config, AuthRepository repository) {
    
    public async Task<string> GetGeneratedTokenAsync<T>(T credentials) where T : IJwtUser {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("jwt:key") ?? string.Empty));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        List<Claim> claims = [new Claim(JwtRegisteredClaimNames.NameId, (await repository.GetUserIdAsync(credentials)).ToString()), 
            new Claim(JwtRegisteredClaimNames.Name, credentials.Username), new Claim("role", credentials.Role)];

        var tokenOptions = new JwtSecurityToken(
            audience: config.GetValue<string>("jwt:audience"),
            issuer: config.GetValue<string>("jwt:issuer"),
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return tokenString;
    }

    public async Task<string> RegisterNewUserAsync(RegisterCredentialsDto credentials) {
        if (await repository.CheckIfUsernameExistAsync(credentials.Username) || await repository.CheckIfEmailExistAsync(credentials.Email)) {
            return string.Empty;
        }
        if(await repository.RegisterUserCredentialsAsync(credentials))
            return await GetGeneratedTokenAsync(credentials);
        return string.Empty;
    }

    public async Task<string> LoginUserAsync(LoginCredentialsDto credentials) {
        
        var cred = await repository.LoginUserAsync(credentials);
        if (cred == false) {
            return string.Empty;
        }
        
        return await GetGeneratedTokenAsync(credentials);
    }

    public async Task<string> CheckIfUsernameOrEmailExistsAsync(string usernameOrEmail) {
        if (await repository.CheckIfUsernameExistAsync(usernameOrEmail) ||
            await repository.CheckIfEmailExistAsync(usernameOrEmail)) {
            return string.Empty;
        }
        return "Available";
    }
}