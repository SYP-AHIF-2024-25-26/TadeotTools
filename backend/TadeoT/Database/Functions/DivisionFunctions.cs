
using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

namespace TadeoT.Database.Functions;
public class DivisionFunctions(TadeoTDbContext context) {
    private readonly TadeoTDbContext context = context;

    public async Task<List<Division>> GetAllDivisions() {
        try {
            return await this.context.Divisions.ToListAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not retrieve Divisions: " + e.Message);
        }
    }

    public async Task<Division> GetDivisionById(int id) {
        Division? division = await this.context.Divisions
            .FirstOrDefaultAsync(d => d.DivisionID == id);
        return division ?? throw new TadeoTNotFoundException("Division not found");
    }

    public async Task<int> GetMaxId() {
        try {
            return !(await this.context.Divisions.AnyAsync()) ? 0 : this.context.Divisions.Max(d => d.DivisionID);
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get MaxId: " + e.Message);
        }
    }

    public async Task<int> AddDivision(Division division) {
        if (division == null) {
            throw new TadeoTArgumentNullException("Could not add Division because it was null");
        }
        try {
            this.context.Divisions.Add(division);
            await this.context.SaveChangesAsync();
            return division.DivisionID;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not add Division: " + e.Message);
        }
    }

    public async void UpdateDivision(Division division) {
        if (division == null) {
            throw new TadeoTArgumentNullException("Could not update Division because it was null");
        }
        try {
            this.context.ChangeTracker.Clear();
            this.context.Divisions.Update(division);
            await this.context.SaveChangesAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not update Division: " + e.Message);
        }
    }

    public async void DeleteDivisionById(int id) {
        try {
            Division division = await this.GetDivisionById(id);
            this.context.Divisions.Remove(division);
            await this.context.SaveChangesAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not delete Division: " + e.Message);
        }
    }

    public async Task<List<Stop>> GetStopsOfDivisionId(int id) {
        try {
            return await this.context.Stops
                .Where(d => d.DivisionID == id)
                .ToListAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get Stops: " + e.Message);
        }
    }
}
