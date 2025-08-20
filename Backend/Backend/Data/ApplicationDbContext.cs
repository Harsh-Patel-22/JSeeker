using Backend.Models;
using Backend.Models.Mapping;
using Backend.Models.Users;
using Backend.Models.Users.Cocurricular;
using Backend.Models.Users.WorkRelated;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options) {
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCredentials> UserCredentials { get; set; }
    public DbSet<Hirer> Hirers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<VocalLanguage> VocalLanguages { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<WorkExperience> WorkExperiences { get; set; }
    public DbSet<Education> Educations { get; set; }
    
    public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
    public DbSet<UserVocalLanguage> UserVocalLanguages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // CONFIGURATION FOR THE PROJECT-TECHNOLOGY JOIN MAPPING
        modelBuilder.Entity<ProjectTechnology>()
            .HasKey(pt => new { pt.ProjectId, pt.TechnologyId });

        modelBuilder.Entity<ProjectTechnology>()
            .HasOne(pt => pt.Project)
            .WithMany(pt => pt.ProjectTechnologies)
            .HasForeignKey(pt => pt.ProjectId);

        modelBuilder.Entity<ProjectTechnology>()
            .HasOne(pt => pt.Technology)
            .WithMany(pt => pt.ProjectTechnologies)
            .HasForeignKey(pt => pt.TechnologyId);


        // CONFIGURATION FOR THE APPLICATION-INTERVIEW 1-TO-1 MAPPING
        modelBuilder.Entity<Application>()
            .HasOne(a => a.Interview)
            .WithOne(i => i.Application)
            .HasForeignKey<Interview>(i => i.ApplicationId);
        
        // CONFIGURATION FOR THE USER-LANGUAGE JOIN MAPPING
        modelBuilder.Entity<UserVocalLanguage>()
            .HasKey(uv => new { uv.UserId, uv.VocalLanguageId });


        modelBuilder.Entity<UserVocalLanguage>()
            .HasOne(uv => uv.User)
            .WithMany(uv => uv.UserVocalLanguages)
            .HasForeignKey(uv => uv.UserId);

        modelBuilder.Entity<UserVocalLanguage>()
            .HasOne(uv => uv.VocalLanguage)
            .WithMany(uv => uv.UserVocalLanguages)
            .HasForeignKey(uv => uv.VocalLanguageId);


        // CONFIGURATION FOR DUAL DELETE RESTRICTIONS
        modelBuilder.Entity<Application>()
            .HasOne(a => a.Hirer)
            .WithMany()
            .HasForeignKey(a => a.HirerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.Seeker)
            .WithMany()
            .HasForeignKey(a => a.SeekerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Interview>()
            .HasOne(i => i.Hirer)
            .WithMany()
            .HasForeignKey(i => i.HirerId)
            .OnDelete(DeleteBehavior.Restrict);

        // modelBuilder.Entity<Interview>()
        //     .HasOne(i => i.Application)
        //     .WithMany()
        //     .HasForeignKey(i => i.ApplicationId)
        //     .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Interview>()
            .HasOne(i => i.Seeker)
            .WithMany()
            .HasForeignKey(i => i.SeekerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Interview>()
            .HasOne(i => i.Job)
            .WithMany()
            .HasForeignKey(i => i.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Hirer)
            .WithMany()
            .HasForeignKey(j => j.HirerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}