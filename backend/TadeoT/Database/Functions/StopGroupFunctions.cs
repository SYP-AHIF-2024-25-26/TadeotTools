using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

namespace TadeoT.Database.Functions;

public class StopGroupFunctions(TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;

    public async Task<List<StopGroup>> GetAllStopGroups()
    {
        try
        {
            return await this.context.StopGroups
                .OrderBy(s => s.StopGroupOrder)
                .ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not retrieve StopGroups: " + e.Message);
        }
    }

    public async Task<StopGroup> GetStopGroupById(int id)
    {
        StopGroup? group = await this.context.StopGroups
            .FirstOrDefaultAsync(sg => sg.StopGroupID == id);
        return group ?? throw new TadeoTNotFoundException("StopGroup not found");
    }

    public async Task<int> GetMaxId()
    {
        try
        {
            return !(await this.context.StopGroups.AnyAsync()) ? 0 : this.context.StopGroups.Max(s => s.StopGroupID);
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get MaxId: " + e.Message);
        }
    }

    public async Task<int> AddStopGroup(StopGroup group)
    {
        if (group == null)
        {
            throw new TadeoTArgumentNullException("Could not add StopGroup because it was null");
        }
        try
        {
            this.context.StopGroups.Add(group);
            await this.context.SaveChangesAsync();
            return group.StopGroupID;
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not add StopGroup: " + e.Message);
        }
    }

    public async Task UpdateStopGroup(StopGroup group)
    {
        if (group == null)
        {
            throw new TadeoTArgumentNullException("Could not update StopGroup because it was null");
        }
        try
        {
            this.context.ChangeTracker.Clear();

            StopGroup sg = this.GetStopGroupById(group.StopGroupID).Result;

            await this.context
                .StopGroups
                .Where(sg => sg.StopGroupID == group.StopGroupID)
                .ExecuteUpdateAsync(g => g
                    .SetProperty(g => g.Name, group.Name)
                    .SetProperty(g => g.Description, group.Description)
                    .SetProperty(g => g.IsPublic, group.IsPublic)
                    .SetProperty(g => g.StopGroupOrder, group.StopGroupOrder)
                );
            // TODO: this.context.StopGroups.ExecuteUpdateAsync(group);
            await this.context.SaveChangesAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not update StopGroup: " + e.Message);
        }
    }

    public async Task DeleteStopGroupById(int id)
    {
        try
        {
            StopGroup group = this.GetStopGroupById(id).Result;
            this.context.StopGroups.Remove(group);
            await this.context.SaveChangesAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not delete StopGroup: " + e.Message);
        }
    }

    public async Task<List<Stop>> GetStopsOfStopGroup(int groupId)
    {
        try
        {
            return await this.context.Stops
                        .Where(s => s.StopGroupID == groupId)
                        .ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get Stops: " + e.Message);
        }
    }

    public async Task MoveStopGroupUp(int groupId)
    {
        StopGroup group = await this.GetStopGroupById(groupId);
        if (group == null) return;

        var aboveItem = await context.StopGroups
            .Where(i => i.StopGroupOrder < group.StopGroupOrder)
            .OrderByDescending(i => i.StopGroupOrder)
            .FirstOrDefaultAsync();

        if (aboveItem != null)
        {
            (aboveItem.StopGroupOrder, group.StopGroupOrder) = (group.StopGroupOrder, aboveItem.StopGroupOrder);
        } else
        {
            group.StopGroupOrder++;
        }
        await context.SaveChangesAsync();
    }

    public async Task MoveStopGroupDown(int stopId)
    {
        StopGroup group = await this.GetStopGroupById(stopId);
        if (group == null) return;

        var aboveItem = await context.StopGroups
            .Where(i => i.StopGroupOrder > group.StopGroupOrder)
            .OrderByDescending(i => i.StopGroupOrder)
            .FirstOrDefaultAsync();

        if (aboveItem != null)
        {
            (aboveItem.StopGroupOrder, group.StopGroupOrder) = (group.StopGroupOrder, aboveItem.StopGroupOrder);
        } else
        {
            group.StopGroupOrder--;
        }
        await context.SaveChangesAsync();
    }
}

