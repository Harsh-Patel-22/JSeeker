using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions options) : base(options) {
        
    }
    
    public DbSet<Location> Locations { get; set; }
    public DbSet<Job> Jobs { get; set; }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        Locations.
    }*/
}