using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

namespace TadeoT.Database.Functions;

public class StopGroupFunctions(TadeoTDbContext context) {
    private readonly TadeoTDbContext context = context;

    public async Task<List<StopGroup>> GetAllStopGroups() {
        try {
            return await this.context.StopGroups.ToListAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not retrieve StopGroups: " + e.Message);
        }
    }

    public async Task<StopGroup> GetStopGroupById(int id) {
        StopGroup? group = await this.context.StopGroups
            .FirstOrDefaultAsync(sg => sg.StopGroupID == id);
        return group ?? throw new TadeoTNotFoundException("StopGroup not found");
    }

    public async Task<int> GetMaxId() {
        try {
            return !(await this.context.StopGroups.AnyAsync()) ? 0 : this.context.StopGroups.Max(s => s.StopGroupID);
        } catch(Exception e) {
            throw new TadeoTDatabaseException("Could not get MaxId: " + e.Message);
        }
    }

    public async Task<int> AddStopGroup(StopGroup group) {
        if (group == null) {
            throw new TadeoTArgumentNullException("Could not add StopGroup because it was null");
        }
        try {
            this.context.StopGroups.Add(group);
            await this.context.SaveChangesAsync();
            return group.StopGroupID;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not add StopGroup: " + e.Message);
        }
    }

    public async void UpdateStopGroup(StopGroup group) {
        if (group == null) {
            throw new TadeoTArgumentNullException("Could not update StopGroup because it was null");
        }
        try {
            this.context.ChangeTracker.Clear();
            this.context.StopGroups.Update(group);
            await this.context.SaveChangesAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not update StopGroup: " + e.Message);
        }
    }

    public async void DeleteStopGroupById(int id) {
        try {
            StopGroup group = this.GetStopGroupById(id).Result;
            this.context.StopGroups.Remove(group);
            await this.context.SaveChangesAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not delete StopGroup: " + e.Message);
        }
    }

    public async Task<List<Stop>> GetStopsOfStopGroup(int groupId) {
        try {
            return await this.context.Stops
                        .Where(s => s.StopGroupID == groupId)
                        .ToListAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not get Stops: " + e.Message);
        }
    }
}

