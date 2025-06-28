using Backend.Models;
using Backend.Models.Mapping;
using Backend.Models.Users;
using Backend.Models.Users.Cocurricular;
using Backend.Models.Users.WorkRelated;
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
    public DbSet<Hobby> Hobbies { get; set; }
    public DbSet<VocalLanguages> VocalLanguages { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<WorkExperience> WorkExperiences { get; set; }
    public DbSet<Education> Educations { get; set; }
    
    public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
    public DbSet<UserHobby> UserHobbies { get; set; }
    public DbSet<UserVocalLanguage> UserVocalLanguages { get; set; }
    
    

    /*protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        Locations.
    }*/
}