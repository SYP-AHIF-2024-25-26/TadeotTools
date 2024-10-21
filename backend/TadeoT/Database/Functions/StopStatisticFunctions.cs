using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database.Functions;

public class StopStatisticFunctions {
    private readonly TadeoTDbContext context = new();

    public StopStatistic GetStopStatisticById(int id) {
        StopStatistic? statistic = this.context.StopStatistics
            .Include(s => s.Stop)
            .FirstOrDefault(s => s.StopStatisticID == id);
        return statistic ?? throw new Exception("StopStatistic not found");
    }

    public int GetMaxId() {
        return this.context.StopStatistics.Max(s => s.StopID);
    }

    public void AddStopStatistic(StopStatistic statistic) {
        try {
            this.context.StopStatistics.Add(statistic);
            this.context.SaveChanges();
        } catch (Exception e) {
            Console.WriteLine("Could not add StopStatistic: " + e.Message);
        }
    }

    public void UpdateStopStatistic(StopStatistic statistic) {
        try {
            this.context.StopStatistics.Update(statistic);
            this.context.SaveChanges();
        } catch (Exception e) {
            Console.WriteLine("Could not update StopStatistic: " + e.Message);
        }
    }

    public void DeleteStopStopStatisticById(int id) {
        try {
            StopStatistic statistic = this.GetStopStatisticById(id);
            this.context.StopStatistics.Remove(statistic);
            this.context.SaveChanges();
        } catch (Exception e) {
            Console.WriteLine("Could not delete StopStatistic: " + e.Message);
        }
    }
    public Stop? GetStopOfStopStatistic(int stopId) {
        try {
            StopStatistic statistic = this.GetStopStatisticById(stopId);
            return statistic.Stop;
        } catch (Exception e) {
            Console.WriteLine("Could not get Stop of StopStatistic: " + e.Message);
            return null;
        }
    }
}

