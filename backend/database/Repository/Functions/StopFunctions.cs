using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Functions;

public record DivisionDto(string Name, string Color);
public record StopGroupDto(string Name, int Order);
public record StopWithAssignmentsAndDivisionsDto(
    int Id,
    string Name,
    string Description,
    List<DivisionDto> Devisions,
    List<StopGroupDto> StopGroups
);

public class StopFunctions(TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;

    public async Task<List<StopWithAssignmentsAndDivisionsDto>> GetAllStopsAync()
    {
        return await context.Stops
            .Select(stop => new StopWithAssignmentsAndDivisionsDto(
                stop.Id,
                stop.Name,
                stop.Description,
                stop.Divisions.Select(d => new DivisionDto(d.Name, d.Color)).ToList(),
                stop.StopGroupAssignments.Select(a => new StopGroupDto(a.StopGroup!.Name, a.Order)).ToList()
                ))
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<List<StopWithAssignmentsAndDivisionsDto>> GetPrivateStopsAync()
    {
        return await context.Stops
            .Where(stop => stop.StopGroupAssignments.Count == 0)
            .Select(stop => new StopWithAssignmentsAndDivisionsDto(
                stop.Id,
                stop.Name,
                stop.Description,
                stop.Divisions.Select(d => new DivisionDto(d.Name, d.Color)).ToList(),
                stop.StopGroupAssignments.Select(a => new StopGroupDto(a.StopGroup!.Name, a.Order)).ToList()
                ))
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<bool> DoesStopExistAsync(int id)
    {
        return await context.Stops.SingleOrDefaultAsync(s => s.Id == id) == null;
    }
}
