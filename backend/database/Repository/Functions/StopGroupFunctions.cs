using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Functions;

public class StopGroupFunctions(TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;

    public record StopGroupWithStops(int Id, string Name, string Description, int Rank, int[] StopIds);

    public async Task<StopGroupWithStops[]> GetAllStopGroupsAsync()
    {
        return [.. context.StopGroups
           .Select(g => new StopGroupWithStops(
               g.Id,
               g.Name,
               g.Description,
               g.Rank,
               context.StopGroupAssignments
                   .Where(a => a.StopGroupId == g.Id)
                   .OrderBy(a => a.Order)
                   .Select(a => a.StopId).ToArray()
           ))];
    }

    public async Task<StopGroupWithStops[]> GetPublicStopGroupsAsync()
    {
        return [.. context.StopGroups
           .Where(g => g.IsPublic)
           .Select(g => new StopGroupWithStops(
               g.Id,
               g.Name,
               g.Description,
               g.Rank,
               context.StopGroupAssignments
                   .Where(a => a.StopGroupId == g.Id)
                   .OrderBy(a => a.Order)
                   .Select(a => a.StopId).ToArray()
           ))];
    }

    public async Task<bool> DoesStopGroupExistAsync(int id)
    {
        return await context.StopGroups.SingleOrDefaultAsync(sg => sg.Id == id) != null;
    }
}
