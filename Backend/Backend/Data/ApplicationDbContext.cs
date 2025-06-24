using Backend.Models;
using Backend.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions options) : base(options) {
        
    }
    
    public DbSet<Location> Locations { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCredentials> UserCredentials { get; set; }
    public DbSet<Hirer> Hirers { get; set; }
    public DbSet<Seeker> Seekers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    

    /*protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        Locations.
    }*/
}