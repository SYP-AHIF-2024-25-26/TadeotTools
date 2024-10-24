﻿using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database.Functions;

public class StopFunctions {
    private readonly TadeoTDbContext context = new();
    private static StopFunctions instance;
    
    private StopFunctions() { }

    public static StopFunctions GetInstance() {
        if (instance == null) {
            instance = new StopFunctions();
        }
        return instance;
    }
    
    public List<Stop> GetAllStops() {
        return [.. this.context.Stops
            .Include(s => s.StopGroup)
            .Include(s => s.StopStatistics)];
    }

    public Stop GetStopById(int id) {
        Stop? stop = this.context.Stops
            .Include(s => s.StopGroup)
            .Include(s => s.StopStatistics)
            .FirstOrDefault(s => s.StopID == id);
        return stop ?? throw new TadeoTDatabaseException("Stop not found");
    }

    public int GetMaxId() {
        return !this.context.Stops.Any() ? 0 : this.context.Stops.Max(s => s.StopID);
    }

    public void AddStop(Stop stop) {
        try {
            this.context.Stops.Add(stop);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not add Stop: " + e.Message);
        }
    }

    public void UpdateStop(Stop stop) {
        try {
            this.context.Stops.Update(stop);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not update Stop: " + e.Message);
        }
    }

    public void DeleteStopById(int id) {
        try {
            Stop stop = this.GetStopById(id);
            this.context.Stops.Remove(stop);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not delete Stop: " + e.Message);
        }
    }

    public StopGroup? GetStopGroupOfStop(int stopId) {
        try {
            Stop stop = this.GetStopById(stopId);
            return stop.StopGroup;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get StopGroup: " + e.Message);
        }
    }

    public List<StopStatistic> GetStopStatisticsOfStop(int stopId) {
        try {
            Stop stop = this.GetStopById(stopId);
            return [.. stop.StopStatistics];
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get StopStatistics: " + e.Message);
        }
    }
}
