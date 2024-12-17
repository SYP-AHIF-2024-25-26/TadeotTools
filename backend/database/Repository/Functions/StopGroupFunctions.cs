using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Functions;

public class StopGroupFunctions(TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;

    public async Task<List<StopGroup>> GetAllStopGroups()
    {
        try
        {
            return await
                context.StopGroups
                .OrderBy(s => s.Rank)
                .ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not retrieve StopGroups: " + e.Message);
        }
    }

    public async Task<StopGroup> GetStopGroupById(int id)
    {
        StopGroup? group = await context.StopGroups
            .SingleOrDefaultAsync(sg => sg.Id == id);
        return group ?? throw new TadeoTNotFoundException("StopGroup not found");
    }

    public async Task<List<Stop>> GetStopsOfStopGroup(int groupId)
    {
        try
        {
            return await context.StopGroupAssignments
                        .Include(s => s.Stop)
                        .Where(s => s.StopGroupId == groupId)
                        .OrderBy(s => s.Order)
                        .Select(sg => sg.Stop!)
                        .ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get Stops: " + e.Message);
        }
    }
}
