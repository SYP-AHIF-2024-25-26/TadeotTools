using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

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
        return [.. this.context.StopStatistics];
    }

    public StopStatistic GetStopStatisticById(int id) {
        StopStatistic? statistic = this.context.StopStatistics
            .FirstOrDefault(s => s.StopStatisticID == id);
        return statistic ?? throw new TadeoTNotFoundException("StopStatistic not found");
    }

    public int GetMaxId() {
        try {
            return !this.context.StopStatistics.Any() ? 0 : this.context.StopStatistics.Max(s => s.StopID);
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get MaxId: " + e.Message);
        }
    }

    public int AddStopStatistic(StopStatistic statistic) {
        if (statistic == null) {
            throw new TadeoTArgumentNullException("Statistic is null");
        }
        try {
            this.context.Entry(statistic.Stop).State = EntityState.Unchanged;
            this.context.StopStatistics.Add(statistic);
            this.context.SaveChanges();
            return statistic.StopStatisticID;
        } catch (TadeoTNotFoundException e) {
            throw new TadeoTNotFoundException("Stopgroup of Stop not found, add it before" + e.Message);
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not add Stop" + e.Message);
        }
    }

    public void UpdateStopStatistic(StopStatistic statistic) {
        if (statistic == null) {
            throw new TadeoTArgumentNullException("Statistic is null");
        }
        try {
            this.context.ChangeTracker.Clear();
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
    public Stop? GetStopOfStopStatistic(int statId) {
        try {
            StopStatistic statistic = this.GetStopStatisticById(statId);
            return StopFunctions.GetInstance().GetStopById(statistic.StopID);
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get Stop: " + e.Message);
        }
    }
    public List<StopStatistic> GetStopStatisticsOfStop(int stopId) {
        try {
            List<StopStatistic> statistics = [.. this.context.StopStatistics.Where(s => s.StopID == stopId)];
            return statistics;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get StopStatistics: " + e.Message);
        }
    }
}
