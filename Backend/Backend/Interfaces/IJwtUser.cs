using Backend.Models.Users;

namespace Backend.Interfaces;

public interface IJwtUser {
    public string Username { get;}
    public string Password { get; }
    public Roles Role { get;}
}