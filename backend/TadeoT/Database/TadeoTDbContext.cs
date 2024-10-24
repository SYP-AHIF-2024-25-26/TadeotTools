﻿using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database;

public class TadeoTDbContext : DbContext {
    public DbSet<StopGroup> StopGroups { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<StopStatistic> StopStatistics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        var connectionString = "Server=localhost;Port=4100;Database=tadeot;User=root;Password=test;";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
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
