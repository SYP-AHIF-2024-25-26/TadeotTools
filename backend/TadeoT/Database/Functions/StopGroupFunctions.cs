using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

namespace TadeoT.Database.Functions;

public class StopGroupFunctions {
    private readonly TadeoTDbContext context = new();

    private static StopGroupFunctions? instance;
    
    private StopGroupFunctions() { }

    public static StopGroupFunctions GetInstance() {
        instance ??= new StopGroupFunctions();
        return instance;
    }
    
    public List<StopGroup> GetAllStopGroups() {
        return [.. this.context.StopGroups];
    }

    public StopGroup GetStopGroupById(int id) {
        StopGroup? group = this.context.StopGroups
            .FirstOrDefault(sg => sg.StopGroupID == id);
        return group ?? throw new TadeoTNotFoundException("StopGroup not found");
    }

    public int GetMaxId() {
        try {
            return !this.context.StopGroups.Any() ? 0 : this.context.StopGroups.Max(s => s.StopGroupID);
        } catch(Exception e) {
            throw new TadeoTDatabaseException("Could not get MaxId: " + e.Message);
        }
    }

    public int AddStopGroup(StopGroup group) {
        if (group == null) {
            throw new TadeoTArgumentNullException("Could not add StopGroup because it was null");
        }
        try {
            this.context.StopGroups.Add(group);
            this.context.SaveChanges();
            return group.StopGroupID;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not add StopGroup: " + e.Message);
        }
    }

    public void UpdateStopGroup(StopGroup group) {
        if (group == null) {
            throw new TadeoTArgumentNullException("Could not update StopGroup because it was null");
        }
        try {
            this.context.ChangeTracker.Clear();
            this.context.StopGroups.Update(group);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not update StopGroup: " + e.Message);
        }
    }

    public void DeleteStopGroupById(int id) {
        try {
            StopGroup group = this.GetStopGroupById(id);
            this.context.StopGroups.Remove(group);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not delete StopGroup: " + e.Message);
        }
    }

    public List<Stop> GetStopsOfStopGroup(int groupId) {
        try {
            return [.. context.Stops.Where(sg => sg.StopGroupID == groupId)];
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get Stops: " + e.Message);
        }
    }
}

