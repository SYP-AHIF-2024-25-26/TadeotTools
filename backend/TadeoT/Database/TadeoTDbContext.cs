using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database;

public class TadeoTDbContext(DbContextOptions<TadeoTDbContext> options) : DbContext(options)
{
    public DbSet<StopGroup> StopGroups { get; set; }
    public DbSet<StopGroupAssignment> StopGroupAssignments { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Division> Divisions { get; set; }
    public DbSet<StopStatistic> StopStatistics { get; set; }
    public DbSet<APIKey> APIKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<APIKey>()
            .HasKey(k => k.APIKeyValue);

        //modelBuilder.Entity<Division>().Property(d => d.Image)
        //          .HasColumnType("varbinary(max)");
    }
}
