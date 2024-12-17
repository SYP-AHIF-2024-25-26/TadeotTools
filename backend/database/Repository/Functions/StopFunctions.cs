using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Functions;

public record DivisionDto(string Name, string Color);
public record StopGroupDto(string Name, int Order);
public record StopWithAssignmentsDto(
    int Id,
    string Name,
    string Description,
    List<DivisionDto> Devisions,
    List<StopGroupDto> StopGroups
    );
public class StopFunctions(TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;

    public async Task<List<StopWithAssignmentsDto>> GetAllStops()
    {
        return await context.Stops
            .Select(stop => new StopWithAssignmentsDto(
                stop.Id,

                stop.Name,
                stop.Description,
                stop.Divisions.Select(d => new DivisionDto(d.Name, d.Color)).ToList(),
                stop.StopGroupAssignments.Select(a => new StopGroupDto(a.StopGroup!.Name, a.Order)).ToList()
                ))
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Stop> GetStopById(int id)
    {
        // TODO: Check if StopGroup and Division are required and loaded correctly
        Stop? stop = await context.Stops
            .Include(s => s.StopGroupAssignments)
            .SingleOrDefaultAsync(s => s.Id == id);
        return stop ?? throw new TadeoTNotFoundException("Stop not found");
    }

    public async Task<List<StopGroup>> GetStopGroupOfStop(int stopId)
    {
        try
        {
            // TODO: rewrite query
            return await context.StopGroupAssignments
                .Include(sg => sg.Stop)
                .Where(sa => sa.StopId == stopId)
                .Select(sa => sa.StopGroup!).ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get StopGroup: " + e.Message);
        }
    }
}
