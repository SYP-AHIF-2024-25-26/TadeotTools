using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

namespace TadeoT.Database;

public class TadeoTDbContext : DbContext
{
    public DbSet<StopGroup> StopGroups { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Division> Divisions { get; set; }
    public DbSet<StopStatistic> StopStatistics { get; set; }
    public DbSet<APIKey> APIKeys { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)  // only configure if not already set by DI
        {
            var serverName = Environment.GetEnvironmentVariable("SERVER_NAME");
            var serverPort = Environment.GetEnvironmentVariable("SERVER_PORT");
            var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
            var username = Environment.GetEnvironmentVariable("DATABASE_USER");
            var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

            var connectionString = $"Server={serverName};Port={serverPort};Database={databaseName};User={username};Password={password};";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

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
