using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

namespace TadeoT.Database.Functions;

public class StopFunctions
{
    private readonly TadeoTDbContext context;
    private readonly StopGroupFunctions stopGroupFunctions;
    private readonly DivisionFunctions divisionFunctions;

    public StopFunctions(
        StopGroupFunctions stopGroupFunctions,
        DivisionFunctions divisionFunctions,
        TadeoTDbContext context
        )
    {
        this.context = context;
        this.stopGroupFunctions = stopGroupFunctions;
        this.divisionFunctions = divisionFunctions;
    }

    public async Task<List<Stop>> GetAllStops()
    {
        return await this.context.Stops
            .Include(s => s.StopGroup)
            .ToListAsync();
    }

    public async Task<Stop> GetStopById(int id)
    {
        Stop? stop = await this.context.Stops
            .Include(s => s.StopGroup)
            .FirstOrDefaultAsync(s => s.StopID == id);
        return stop ?? throw new TadeoTNotFoundException("Stop not found");
    }

    public async Task<int> GetMaxId()
    {
        try
        {
            return !(await this.context.Stops.AnyAsync()) ? 0 : this.context.Stops.Max(s => s.StopID);
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get MaxId: " + e.Message);
        }
    }

    public async Task<int> AddStop(Stop stop)
    {
        if (stop == null)
        {
            throw new TadeoTArgumentNullException("Stop is null");
        }
        try
        {
            if (stop.StopGroup != null)
            {
                var existingStopGroup = await this.stopGroupFunctions.GetStopGroupById(stop.StopGroup.StopGroupID);
                stop.StopGroupID = existingStopGroup.StopGroupID;
                this.context.Entry(stop.StopGroup).State = EntityState.Unchanged;
            }
            if (stop.Division != null)
            {
                var existingDivision = await this.divisionFunctions.GetDivisionById(stop.Division.DivisionID);
                stop.DivisionID = existingDivision.DivisionID;
                this.context.Entry(stop.Division).State = EntityState.Unchanged;
            }
            this.context.Stops.Add(stop);
            await this.context.SaveChangesAsync();
            return stop.StopID;
        } catch (TadeoTNotFoundException e)
        {
            throw new TadeoTNotFoundException("Stopgroup of Stop not found, add it before" + e.Message);
        } catch (Exception)
        {
            throw new TadeoTDatabaseException("Could not add Stop");
        }
    }

    public async void UpdateStop(Stop stop)
    {
        if (stop == null)
        {
            throw new TadeoTArgumentNullException("Stop is null");
        }
        try
        {
            this.context.ChangeTracker.Clear();
            this.context.Stops.Update(stop);
            await this.context.SaveChangesAsync();
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not update Stop: " + e.Message);
        }
    }

    public async void DeleteStopById(int id)
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

    public async Task<StopGroup?> GetStopGroupOfStop(int stopId)
    {
        try
        {
            Stop stop = await this.GetStopById(stopId);
            return stop.StopGroup;
        } catch (Exception e)
        {
            throw new TadeoTDatabaseException("Could not get StopGroup: " + e.Message);
        }
    }
}
