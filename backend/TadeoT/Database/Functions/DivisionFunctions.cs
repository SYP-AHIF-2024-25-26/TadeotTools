
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database.Functions;
public class DivisionFunctions(TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;

    public async Task<List<Division>> GetAllDivisions()
    {
        try
        {
            return await this.context.Divisions.ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not retrieve Divisions: " + e.Message);
        }
    }

    public async Task<Division> GetDivisionById(int id)
    {
        Division? division = await this.context.Divisions
            .SingleOrDefaultAsync(d => d.Id == id);
        return division ?? throw new TadeoTNotFoundException("Division not found");
    }

    public async Task<int> AddDivision(Division division)
    {
        // TODO: Add validation! division cannot be null by design
        if (division == null)
        {
            throw new TadeoTArgumentNullException("Could not add Division because it was null");
        }
        try
        {
            this.context.Divisions.Add(division);
            await this.context.SaveChangesAsync();
            return division.Id;
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not add Division: " + e.Message);
        }
    }

    public async Task UpdateDivision(Division division)
    {
        // TODO: Add validation! division cannot be null by design
        if (division == null)
        {
            throw new TadeoTArgumentNullException("Could not update Division because it was null");
        }
        try
        {
            await this.context
                .Divisions
                .Where(d => d.Id == division.Id)
                .ExecuteUpdateAsync(d => d
                    .SetProperty(d => d.Name, division.Name)
                    .SetProperty(d => d.Color, division.Color)
                    .SetProperty(d => d.Image, division.Image));
            await this.context.SaveChangesAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not update Division: " + e.Message);
        }
    }

    public async Task DeleteDivisionById(int id)
    {
        try
        {
            Division division = await this.GetDivisionById(id);
            this.context.Divisions.Remove(division);
            await this.context.SaveChangesAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not delete Division: " + e.Message);
        }
    }

    public async Task<List<Stop>> GetStopsOfDivisionId(int id)
    {
        try
        {
            return await this.context.Stops
                .Where(s => s.Divisions.Any(d => d.Id == id))
                .ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get Stops: " + e.Message);
        }
    }
}
