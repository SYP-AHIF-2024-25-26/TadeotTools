using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace TadeoT.Database.Functions;

public class StopFunctions(
    StopGroupFunctions stopGroupFunctions,
    DivisionFunctions divisionFunctions,
    TadeoTDbContext context
        )
{
    private readonly TadeoTDbContext context = context;
    private readonly StopGroupFunctions stopGroupFunctions = stopGroupFunctions;
    private readonly DivisionFunctions divisionFunctions = divisionFunctions;

    public async Task<List<Stop>> GetAllStops()
    {
        // TODO: Create a DTO for Stop to avoid circular references, and load associated Stop group into it
        return await this.context.Stops
            .Include(s => s.StopGroupAssignments)
            //.OrderBy(s => s.StopOrder)
            .ToListAsync();
    }

    public async Task<Stop> GetStopById(int id)
    {
        // TODO: Check if StopGroup and Division are required and loaded correctly
        Stop? stop = await this.context.Stops
            .Include(s => s.StopGroupAssignments)
            .SingleOrDefaultAsync(s => s.Id == id);
        return stop ?? throw new TadeoTNotFoundException("Stop not found");
    }

    public async Task<int> AddStop(Stop stop)
    {
        if (stop == null)
        {
            throw new TadeoTArgumentNullException("Stop is null");
        }
        try
        {
            // TODO: handle relations to StopGroup and Division separately!!

            //if (stop.StopGroup != null)
            //{
            //    var existingStopGroup = await this.stopGroupFunctions.GetStopGroupById(stop.StopGroup.StopGroupID);
            //    stop.StopGroupID = existingStopGroup.StopGroupID;
            //    stop.StopGroup = null; // detach to avoid double tracking
            //    //this.context.Entry(stop.StopGroup).State = EntityState.Unchanged;
            //}
            //if (stop.Division != null)
            //{
            //    var existingDivision = await this.divisionFunctions.GetDivisionById(stop.Division.DivisionID);
            //    stop.DivisionID = existingDivision.DivisionID;
            //    stop.Division = null;
            //    //this.context.Entry(stop.Division).State = EntityState.Unchanged;
            //}
            this.context.Stops.Add(stop);
            await this.context.SaveChangesAsync();
            return stop.Id;
        } catch (TadeoTNotFoundException e)
        {
            throw new TadeoTNotFoundException("Stopgroup of Stop not found, add it before" + e.Message);
        } catch (Exception)
        {
            throw new TadeoTDatabaseException("Could not add Stop");
        }
    }

    public async Task UpdateStop(Stop stop)
    {
        if (stop == null)
        {
            throw new TadeoTArgumentNullException("Stop is null");
        }
        try
        {
            // TODO: Update Rank of stopgroupassignment separately, also division relationship
            await this.context
                .Stops
                .Where(s => s.Id == stop.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.Name, stop.Name)
                    .SetProperty(s => s.Description, stop.Description)
                    //.SetProperty(s => s.Rank, stop.StopOrder)
                    //.SetProperty(s => s.StopGroupID, stop.StopGroupID)
                    //.SetProperty(s => s.DivisionID, stop.DivisionID)
                );
            await this.context.SaveChangesAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not update Stop: " + e.Message);
        }
    }

    public async Task DeleteStopById(int id)
    {
        try
        {
            Stop stop = await this.GetStopById(id);
            this.context.Stops.Remove(stop);
            await this.context.SaveChangesAsync();
        } catch (TadeoTNotFoundException e)
        {
            throw new TadeoTNotFoundException("Stop not found, add it before deleting" + e.Message);
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not delete Stop: " + e.Message);
        }
    }

    public async Task<List<StopGroup>> GetStopGroupOfStop(int stopId)
    {
        try
        {
            // TODO: rewrite query
            return await context.StopGroupAssignments
                .Include(sg => sg.Stop)
                .Where(sa => sa.StopId == stopId)
                .Select(sa => sa.StopGroup!).ToListAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get StopGroup: " + e.Message);
        }
    }

    public Task MoveStopUp(int stopId)
    {
        return Task.CompletedTask;
        // TODO: API Change required! Move a stopgroupassignment up or down instead!

        //Stop stop = await this.GetStopById(stopId);
        //if (stop == null) return;

        //var aboveItem = await context.Stops
        //    .Where(i => i.Rank < stop.Rank)
        //    .OrderByDescending(i => i.StopOrder)
        //    .FirstOrDefaultAsync();

        //if (aboveItem != null)
        //{
        //    (aboveItem.StopOrder, stop.Rank) = (stop.Rank, aboveItem.StopOrder);
        //} else
        //{
        //    stop.Rank++;
        //}
        //await context.SaveChangesAsync();
    }

    public  Task MoveStopDown(int stopId)
    {
        return Task.CompletedTask;
        // TODO: API Change required! Move a stopgroupassignment up or down instead!
        /*
        Stop stop = await this.GetStopById(stopId);
        if (stop == null) return;

        var aboveItem = await context.Stops
            .Where(i => i.StopOrder > stop.StopOrder)
            .OrderByDescending(i => i.StopOrder)
            .FirstOrDefaultAsync();

        if (aboveItem != null)
        {
            (aboveItem.StopOrder, stop.StopOrder) = (stop.StopOrder, aboveItem.StopOrder);
        } else
        {
            stop.StopOrder--;
        }
        await context.SaveChangesAsync();
        **/
    }
}
