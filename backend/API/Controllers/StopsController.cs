using API.Dtos.ResponseDtos;
using API.RequestDto;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

namespace API.Controllers;

[ApiController]
[Route("v1")]
public class StopsController(
    StopFunctions stops,
    StopGroupFunctions stopGroups,
    DivisionFunctions divisions
) : ControllerBase
{
    [HttpGet("api/stops")]
    public async Task<IResult> GetAllStops()
    {
        var allStops = await stops.GetAllStops();
        List<ResponseStopDto> responseStops = new();
        allStops.ForEach(stop => { responseStops.Add(ResponseStopDto.FromStop(stop)); });
        return Results.Ok(responseStops);
    }

    [HttpGet("api/stops/{stopId}")]
    public async Task<IResult> GetStopById(int stopId)
    {
        try
        {
            var stop = await stops.GetStopById(stopId);
            return Results.Ok(ResponseStopDto.FromStop(stop));
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("Stop not found");
        }
    }

    [HttpPost("api/stops")]
    public async Task<IResult> CreateStop([FromBody] RequestStopDto stop)
    {
        try
        {
            try
            {
                await stopGroups.GetStopGroupById(stop.StopGroupID);
            }
            catch (TadeoTNotFoundException)
            {
                return Results.NotFound("StopGroup not found");
            }

            if (stop.Name.Length > 50)
            {
                return Results.BadRequest("Invalid Name");
            }

            if (stop.Description.Length > 255)
            {
                return Results.BadRequest("Invalid Description");
            }

            if (stop.RoomNr.Length > 5)
            {
                return Results.BadRequest("Invalid Room Number");
            }

            var stopToAdd = new Stop
            {
                Name = stop.Name,
                Description = stop.Description,
                RoomNr = stop.RoomNr,
                Division = await divisions.GetDivisionById(stop.DivisionID),
                StopGroup = await stopGroups.GetStopGroupById(stop.StopGroupID)
            };

            stopToAdd.StopID = await stops.AddStop(stopToAdd);

            return Results.Ok(ResponseStopDto.FromStop(stopToAdd));
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("Division not found");
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }

    [HttpPut("api/stops/{stopId}")]
    public async Task<IResult> UpdateStop(int stopId, [FromBody] RequestStopDto stop)
    {
        try
        {
            try
            {
                await stopGroups.GetStopGroupById(stop.StopGroupID);
            }
            catch (TadeoTNotFoundException)
            {
                return Results.NotFound("StopGroup not found");
            }

            try
            {
                await divisions.GetDivisionById(stop.DivisionID);
            }
            catch (TadeoTNotFoundException)
            {
                return Results.NotFound("Division not found");
            }

            if (stop.Name.Length > 50)
            {
                return Results.NotFound("Invalid Name");
            }

            if (stop.Description.Length > 255)
            {
                return Results.NotFound("Invalid Description");
            }

            if (stop.RoomNr.Length > 5)
            {
                return Results.NotFound("Invalid RoomNr");
            }

            await stops.GetStopById(stopId);

            await stops.UpdateStop(new Stop
            {
                StopID = stopId,
                Name = stop.Name,
                Description = stop.Description,
                RoomNr = stop.RoomNr,
                Division = await divisions.GetDivisionById(stop.DivisionID),
                StopGroup = await stopGroups.GetStopGroupById(stop.StopGroupID)
            });
            return Results.Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("Stop not found");
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }

    [HttpDelete("api/stops/{stopId}")]
    public async Task<IResult> DeleteStop(int stopId)
    {
        try
        {
            await stops.GetStopById(stopId);
            await stops.DeleteStopById(stopId);
            return Results.Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("Stop not found");
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }

    [HttpGet("stops/groups/{groupId}")]
    public async Task<IResult> GetStopsByGroupId(int groupId)
    {
        try
        {
            await stopGroups.GetStopGroupById(groupId);
            List<ResponseStopDto> responseStops = new();
            var stopsByGroup = await stopGroups.GetStopsOfStopGroup(groupId);
            stopsByGroup.ForEach(stop => { responseStops.Add(ResponseStopDto.FromStop(stop)); });
            return Results.Ok(responseStops);
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("StopGroup not found");
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }
}