using database.Entities;
using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database.Functions;

public class StopStatisticFunctions(StopFunctions stopFunctions, TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;
    private readonly StopFunctions stopFunctions = stopFunctions;

    public async Task<List<StopStatistic>> GetAllStopStatistics()
    {
        return await this.context.StopStatistics.ToListAsync();
    }

    public async Task<StopStatistic> GetStopStatisticById(int id)
    {
        StopStatistic? statistic = await this.context.StopStatistics
            .FirstOrDefaultAsync(s => s.Id == id);
        return statistic ?? throw new TadeoTNotFoundException("StopStatistic not found");
    }

    public async Task<int> AddStopStatistic(StopStatistic statistic)
    {
        if (statistic == null)
        {
            throw new TadeoTArgumentNullException("Statistic is null");
        }
        try
        {
            // TODO: WTF are you doing here?
            //this.context.Entry(statistic.Stop).State = EntityState.Unchanged;
            this.context.StopStatistics.Add(statistic);
            await this.context.SaveChangesAsync();
            return statistic.Id;
        } catch (TadeoTNotFoundException e)
        {
            throw new TadeoTNotFoundException("Stopgroup of Stop not found, add it before: " + e.Message);
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not add Stop: " + e.Message);
        }
    }

    public async Task DeleteStopStopStatisticById(int id)
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
            return await stopFunctions.GetStopById(statistic.StopId);
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get Stop: " + e.Message);
        }
    }
    public async Task<List<StopStatistic>> GetStopStatisticsOfStop(int stopId)
    {
        try
        {
            return await this.context.StopStatistics.Where(s => s.StopId == stopId).ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get StopStatistics: " + e.Message);
        }
    }
}
