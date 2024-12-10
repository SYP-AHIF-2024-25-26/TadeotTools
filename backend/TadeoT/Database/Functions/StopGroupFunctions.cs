using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database.Functions;

public class StopGroupFunctions(TadeoTDbContext context)
{
    private readonly TadeoTDbContext context = context;

    public async Task<List<StopGroup>> GetAllStopGroups()
    {
        try
        {
            return await this.context.StopGroups
                .OrderBy(s => s.Rank)
                .ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not retrieve StopGroups: " + e.Message);
        }
    }

    public async Task<StopGroup> GetStopGroupById(int id)
    {
        StopGroup? group = await this.context.StopGroups
            .SingleOrDefaultAsync(sg => sg.Id == id);
        return group ?? throw new TadeoTNotFoundException("StopGroup not found");
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
            return group.Id;
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
            await this.context
                .StopGroups
                .Where(sg => sg.Id == group.Id)
                .ExecuteUpdateAsync(g => g
                    .SetProperty(g => g.Name, group.Name)
                    .SetProperty(g => g.Description, group.Description)
                    .SetProperty(g => g.IsPublic, group.IsPublic)
                    .SetProperty(g => g.Rank, group.Rank)
                );
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
            StopGroup group = await this.GetStopGroupById(id);
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
            return await this.context.StopGroupAssignments
                        .Include(s => s.Stop)   
                        .Where(s => s.StopGroupId == groupId)
                        .OrderBy(s => s.Order)
                        .Select(sg => sg.Stop!)
                        .ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get Stops: " + e.Message);
        }
    }

    public async Task MoveStopGroupUp(int groupId)
    {
        // TODO: messy! Refactor this
        var group = await this.GetStopGroupById(groupId);

        var aboveItem = await context.StopGroups
            .Where(i => i.Rank < group.Rank)
            .OrderByDescending(i => i.Rank)
            .FirstOrDefaultAsync();

        if (aboveItem != null)
        {
            (aboveItem.Rank, group.Rank) = (group.Rank, aboveItem.Rank);
        } else
        {
            group.Rank++;
        }
        await context.SaveChangesAsync();
    }

    public async Task MoveStopGroupDown(int stopId)
    {
        // TODO: messy! Refactor this

        var group = await this.GetStopGroupById(stopId);

        var aboveItem = await context.StopGroups
            .Where(i => i.Rank > group.Rank)
            .OrderByDescending(i => i.Rank)
            .FirstOrDefaultAsync();

        if (aboveItem != null)
        {
            (aboveItem.Rank, group.Rank) = (group.Rank, aboveItem.Rank);
        } else
        {
            group.Rank--;
        }
        await context.SaveChangesAsync();
    }
}

