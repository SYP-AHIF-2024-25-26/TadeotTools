using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database.Functions;

public class StopStatisticFunctions {
    private readonly TadeoTDbContext context = new();

    private static StopStatisticFunctions? instance;
    
    private StopStatisticFunctions() { }

    public static StopStatisticFunctions GetInstance() {
        instance ??= new StopStatisticFunctions();
        return instance;
    }
    
    public List<StopStatistic> GetAllStopStatistics() {
        return [.. this.context.StopStatistics.Include(s => s.Stop)];
    }

    public StopStatistic GetStopStatisticById(int id) {
        StopStatistic? statistic = this.context.StopStatistics
            .Include(s => s.Stop)
            .FirstOrDefault(s => s.StopStatisticID == id);
        return statistic ?? throw new TadeoTDatabaseException("StopStatistic not found");
    }

    public int GetMaxId() {
        try {
            return !this.context.StopStatistics.Any() ? 0 : this.context.StopStatistics.Max(s => s.StopID);
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get MaxId: " + e.Message);
        }
    }

    public int AddStopStatistic(StopStatistic statistic) {
        try {
            this.context.StopStatistics.Add(statistic);
            this.context.SaveChanges();
            return statistic.StopStatisticID;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not add StopStatistic: " + e.Message);
        }
    }

    public void UpdateStopStatistic(StopStatistic statistic) {
        try {
            this.context.StopStatistics.Update(statistic);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not update StopStatistic: " + e.Message);
        }
    }

    public void DeleteStopStopStatisticById(int id) {
        try {
            StopStatistic statistic = this.GetStopStatisticById(id);
            this.context.StopStatistics.Remove(statistic);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not delete StopStatistic: " + e.Message);
        }
    }
    public Stop? GetStopOfStopStatistic(int stopId) {
        try {
            StopStatistic statistic = this.GetStopStatisticById(stopId);
            return statistic.Stop;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get Stop: " + e.Message);
        }
    }
}

