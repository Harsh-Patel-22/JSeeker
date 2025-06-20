using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions options) : base(options) {
        
    }
    
    public DbSet<Location> Locations { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Application> Applications { get; set; }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        Locations.
    }*/
}