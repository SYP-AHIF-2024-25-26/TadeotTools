using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Functions;

public class StopStatisticFunctions(StopFunctions stopFunctions, TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;
    private readonly StopFunctions stopFunctions = stopFunctions;

    public async Task<List<StopStatistic>> GetAllStopStatistics()
    {
        return await context.StopStatistics.ToListAsync();
    }

    public async Task<StopStatistic> GetStopStatisticById(int id)
    {
        StopStatistic? statistic = await context.StopStatistics
            .FirstOrDefaultAsync(s => s.Id == id);
        return statistic ?? throw new TadeoTNotFoundException("StopStatistic not found");
    }

    public async Task DeleteStopStopStatisticById(int id)
    {
        try
        {
            StopStatistic statistic = await GetStopStatisticById(id);
            context.StopStatistics.Remove(statistic);
            await context.SaveChangesAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not delete StopStatistic: " + e.Message);
        }
    }
    public async Task<Stop?> GetStopOfStopStatistic(int statId)
    {
        try
        {
            StopStatistic statistic = await GetStopStatisticById(statId);
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
            return await context.StopStatistics.Where(s => s.StopId == stopId).ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get StopStatistics: " + e.Message);
        }
    }
}
