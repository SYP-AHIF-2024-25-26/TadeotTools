using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

namespace TadeoT.Database;

public class TadeoTDbContext(DbContextOptions<TadeoTDbContext> options) : DbContext(options)
{
    public DbSet<StopGroup> StopGroups { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Division> Divisions { get; set; }
    public DbSet<StopStatistic> StopStatistics { get; set; }
    public DbSet<APIKey> APIKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stop>()
            .HasOne(s => s.StopGroup)
            .WithMany()
            .HasForeignKey(s => s.StopGroupID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Stop>()
            .HasOne(s => s.Division)
            .WithMany()
            .HasForeignKey(s => s.DivisionID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StopStatistic>()
            .HasOne(stat => stat.Stop)
            .WithMany()
            .HasForeignKey(stat => stat.StopID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<APIKey>()
            .HasKey(k => k.APIKeyValue);
    }
}
