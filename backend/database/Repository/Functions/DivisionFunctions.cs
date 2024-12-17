using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Database.Repository.Functions;
public class DivisionFunctions(TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;

    public record DivisionWithoutImageDto(int Id, string Name, string Color);
    public async Task<List<DivisionWithoutImageDto>> GetAllDivisionsWithoutImageAsync()
    {
        return await
            context.Divisions
            .Select(d => new DivisionWithoutImageDto(d.Id, d.Name, d.Color))
            .ToListAsync();
    }

    public async Task<Division?> GetDivisionById(int id)
    {
        Division? division = await context.Divisions
            .SingleOrDefaultAsync(d => d.Id == id);
        return division;
    }

    public async Task<List<Stop>> GetStopsOfDivisionId(int id)
    {
        try
        {
            return await context.Stops
                .Where(s => s.Divisions.Any(d => d.Id == id))
                .ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get Stops: " + e.Message);
        }
    }
}
