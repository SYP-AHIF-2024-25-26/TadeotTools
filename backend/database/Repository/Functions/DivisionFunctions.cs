using Microsoft.EntityFrameworkCore;

namespace Database.Repository.Functions;
public class DivisionFunctions(TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;

    public record DivisionWithoutImageDto(int Id, string Name, string Color);
    public record DivisionWithImageDto(int Id, string Name, byte[]? Image);

    public async Task<List<DivisionWithoutImageDto>> GetAllDivisionsWithoutImageAsync()
    {
        return await
            context.Divisions
            .Select(d => new DivisionWithoutImageDto(d.Id, d.Name, d.Color))
            .ToListAsync();
    }

    // TODO: do we need that?
    public async Task<List<DivisionWithImageDto>> GetAllDivisionsWithImageAsync()
    {
        return await
            context.Divisions
            .Select(d => new DivisionWithImageDto(d.Id, d.Name, d.Image))
            .ToListAsync();
    }

    public async Task<byte[]?> GetImageOfDivision(int divisionId)
    {
        return await context.Divisions
            .Where(d => d.Id == divisionId)
            .Select(d => d.Image)
            .SingleOrDefaultAsync();
    }

    public async Task<bool> DoesDivisionExistAsync(int id)
    {
        return await context.Divisions.SingleOrDefaultAsync(d => d.Id == id) != null;
    }
}
