using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TadeoT;

public class StopGroup
{
    public int StopGroupID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Color { get; set; }

    public required ICollection<Stop> Stops { get; set; }
}

public class Stop
{
    public int StopID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string RoomNr { get; set; }
    public int? StopGroupID { get; set; }

    public required StopGroup StopGroup { get; set; }

    public required ICollection<StopStatistic> StopStatistics { get; set; }
}

public class StopStatistic
{
    public required int StopStatisticID { get; set; }
    public required DateTime Time { get; set; }
    public required bool IsDone { get; set; }
    public required int StopID { get; set; }

    public required Stop Stop { get; set; }
}

public class MyDbContext : DbContext
{
    public DbSet<StopGroup> StopGroups { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<StopStatistic> StopStatistics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost;Port=4100;Database=tadeot;User=root;Password=test;";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stop>()
            .HasOne(s => s.StopGroup)
            .WithMany(sg => sg.Stops)
            .HasForeignKey(s => s.StopGroupID);

        modelBuilder.Entity<StopStatistic>()
            .HasOne(ss => ss.Stop)
            .WithMany(s => s.StopStatistics)
            .HasForeignKey(ss => ss.StopID);
    }
}

class Program
{
    static void Main(string[] args)
    {
        using var context = new MyDbContext();
        context.Database.EnsureCreated();

        // Test example
        var stopGroup = new StopGroup
        {
            Name = "Main Building",
            Description = "Group for stops in the main building",
            Color = "#FF5733",
            Stops = []
        };

        var stop = new Stop
        {
            Name = "Room 101",
            Description = "Meeting room 101",
            RoomNr = "101",
            StopGroup = stopGroup,
            StopStatistics = []
        };

        context.StopGroups.Add(stopGroup);
        context.Stops.Add(stop);

        context.SaveChanges();

        var stopGroups = context.StopGroups.Include(sg => sg.Stops).ToList();
        foreach (var sg in stopGroups)
        {
            Console.WriteLine($"StopGroup: {sg.Name}, Color: {sg.Color}");
            foreach (var s in sg.Stops)
            {
                Console.WriteLine($"    Stop: {s.Name}, RoomNr: {s.RoomNr}");
            }
        }
    }
}
