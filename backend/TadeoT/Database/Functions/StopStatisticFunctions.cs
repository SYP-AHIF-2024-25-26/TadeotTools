using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

namespace TadeoT.Database.Functions;

public class StopStatisticFunctions
{
    private readonly TadeoTDbContext context;
    private readonly StopFunctions stopFunctions;

    public StopStatisticFunctions(StopFunctions stopFunctions, TadeoTDbContext context)
    {
        this.stopFunctions = stopFunctions;
        this.context = context;
    }

    public async Task<List<StopStatistic>> GetAllStopStatistics()
    {
        return await this.context.StopStatistics.ToListAsync();
    }

    public async Task<StopStatistic> GetStopStatisticById(int id)
    {
        StopStatistic? statistic = await this.context.StopStatistics
            .FirstOrDefaultAsync(s => s.StopStatisticID == id);
        return statistic ?? throw new TadeoTNotFoundException("StopStatistic not found");
    }

    public async Task<int> GetMaxId()
    {
        try
        {
            return !(await this.context.StopStatistics.AnyAsync()) ? 0 : this.context.StopStatistics.Max(s => s.StopStatisticID);
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get MaxId: " + e.Message);
        }
    }

    public async Task<int> AddStopStatistic(StopStatistic statistic)
    {
        if (statistic == null)
        {
            throw new TadeoTArgumentNullException("Statistic is null");
        }
        try
        {
            this.context.Entry(statistic.Stop).State = EntityState.Unchanged;
            this.context.StopStatistics.Add(statistic);
            await this.context.SaveChangesAsync();
            return statistic.StopStatisticID;
        } catch (TadeoTNotFoundException e)
        {
            throw new TadeoTNotFoundException("Stopgroup of Stop not found, add it before" + e.Message);
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not add Stop" + e.Message);
        }
    }

    public async void UpdateStopStatistic(StopStatistic statistic)
    {
        if (statistic == null)
        {
            throw new TadeoTArgumentNullException("Statistic is null");
        }
        try
        {
            this.context.ChangeTracker.Clear();
            this.context.StopStatistics.Update(statistic);
            await this.context.SaveChangesAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not update StopStatistic: " + e.Message);
        }
    }

    public async void DeleteStopStopStatisticById(int id)
    {
        try
        {
            StopStatistic statistic = await this.GetStopStatisticById(id);
            this.context.StopStatistics.Remove(statistic);
            await this.context.SaveChangesAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not delete StopStatistic: " + e.Message);
        }
    }
    public async Task<Stop?> GetStopOfStopStatistic(int statId)
    {
        try
        {
            StopStatistic statistic = await this.GetStopStatisticById(statId);
            return await stopFunctions.GetStopById(statistic.StopID);
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get Stop: " + e.Message);
        }
    }
    public async Task<List<StopStatistic>> GetStopStatisticsOfStop(int stopId)
    {
        try
        {
            return await this.context.StopStatistics.Where(s => s.StopID == stopId).ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get StopStatistics: " + e.Message);
        }
    }
}
