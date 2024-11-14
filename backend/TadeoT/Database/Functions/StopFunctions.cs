using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database.Functions;

public class StopFunctions {
    private readonly TadeoTDbContext context = new();
    private static StopFunctions? instance;
    
    private StopFunctions() { }

    public static StopFunctions GetInstance() {
        instance ??= new StopFunctions();
        return instance;
    }
    
    public List<Stop> GetAllStops() {
        return [.. this.context.Stops
            .Include(s => s.StopGroup)];
    }

    public Stop GetStopById(int id) {
        Stop? stop = this.context.Stops
            .Include(s => s.StopGroup)
            .FirstOrDefault(s => s.StopID == id);
        return stop ?? throw new TadeoTNotFoundException("Stop not found");
    }

    public int GetMaxId() {
        return !this.context.Stops.Any() ? 0 : this.context.Stops.Max(s => s.StopID);
    }

    public int AddStop(Stop stop) {
        if (stop == null) {
            throw new TadeoTArgumentNullException("Stop is null");
        }
        try {
            if (stop.StopGroup != null) {
                var existingStopGroup = StopGroupFunctions.GetInstance().GetStopGroupById(stop.StopGroup.StopGroupID);
                stop.StopGroupID = existingStopGroup.StopGroupID;
                this.context.Entry(stop.StopGroup).State = EntityState.Unchanged;
            }
            if (stop.Division != null) {
                var existingDivision = DivisionFunctions.GetInstance().GetDivisionById(stop.Division.DivisionID);
                stop.DivisionID = existingDivision.DivisionID;
                this.context.Entry(stop.Division).State = EntityState.Unchanged;
            }
            this.context.Stops.Add(stop);
            this.context.SaveChanges();
            return stop.StopID;
        } catch (TadeoTNotFoundException e) {
            throw new TadeoTNotFoundException("Stopgroup of Stop not found, add it before" + e.Message);
        } catch (Exception) {
            throw new TadeoTDatabaseException("Could not add Stop");
        }
    }

    public void UpdateStop(Stop stop) {
        if (stop == null) {
            throw new TadeoTArgumentNullException("Stop is null");
        }
        try {
            this.context.ChangeTracker.Clear();
            this.context.Stops.Update(stop);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not update Stop: " + e.Message);
        }
    }

    public void DeleteStopById(int id) {
        try {
            Stop stop = this.GetStopById(id);
            this.context.Stops.Remove(stop);
            this.context.SaveChanges();
        } catch (TadeoTNotFoundException e) {
            throw new TadeoTNotFoundException("Stop not found, add it before deleting" + e.Message);
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not delete Stop: " + e.Message);
        }
    }

    public StopGroup? GetStopGroupOfStop(int stopId) {
        try {
            Stop stop = this.GetStopById(stopId);
            return stop.StopGroup;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get StopGroup: " + e.Message);
        }
    }
}
