
using TadeoT.Database.Model;

namespace TadeoT.Database.Functions;
public class DivisionFunctions {
    private readonly TadeoTDbContext context = new();

    private static DivisionFunctions? instance;

    private DivisionFunctions() { }

    public static DivisionFunctions GetInstance() {
        instance ??= new DivisionFunctions();
        return instance;
    }

    public List<Division> GetAllDivisions() {
        return [.. this.context.Divisions];
    }

    public Division GetDivisionById(int id) {
        Division? division = this.context.Divisions
            .FirstOrDefault(d => d.DivisionID == id);
        return division ?? throw new TadeoTNotFoundException("Division not found");
    }

    public int GetMaxId() {
        try {
            return !this.context.Divisions.Any() ? 0 : this.context.Divisions.Max(d => d.DivisionID);
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get MaxId: " + e.Message);
        }
    }

    public int AddDivision(Division division) {
        if (division == null) {
            throw new TadeoTArgumentNullException("Could not add Division because it was null");
        }
        try {
            this.context.Divisions.Add(division);
            this.context.SaveChanges();
            return division.DivisionID;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not add Division: " + e.Message);
        }
    }

    public void UpdateDivision(Division division) {
        if (division == null) {
            throw new TadeoTArgumentNullException("Could not update Division because it was null");
        }
        try {
            this.context.ChangeTracker.Clear();
            this.context.Divisions.Update(division);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not update Division: " + e.Message);
        }
    }

    public void DeleteDivisionById(int id) {
        try {
            Division division = this.GetDivisionById(id);
            this.context.Divisions.Remove(division);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not delete Division: " + e.Message);
        }
    }

    public List<Stop> GetStopsOfDivisionId(int id) {
        try {
            return [.. context.Stops.Where(d => d.DivisionID == id)];
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get Stops: " + e.Message);
        }
    }
}
