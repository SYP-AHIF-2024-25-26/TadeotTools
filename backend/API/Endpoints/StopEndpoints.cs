using API.Dtos.RequestDtos;
using API.Dtos.ResponseDtos;
using API.RequestDto;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

namespace API.Endpoints;

public static class StopEndpoints
{
    public static void MapStopEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("v1");
        group.MapGet("api/groups", GetAllStops);
        group.MapGet("api/stops/private", GetPrivateStops);
        group.MapGet("api/stops/{stopId}", GetStopById);
        group.MapPost("api/stops", CreateStop);
        group.MapPut("api/stops/{stopId}", UpdateStop);
        group.MapDelete("api/stops/{stopId}", DeleteStop);
        group.MapGet("stops/groups/{groupId}", GetStopsByGroupId);
        group.MapPut("api/stops/order", UpdateOrder);
    }

    public static async Task<IResult> GetAllStops(
        StopFunctions stops
    )
    {
        var allStops = await stops.GetAllStops();
        List<ResponseStopDto> responseStops = new();
        allStops.ForEach(stop => { responseStops.Add(ResponseStopDto.FromStop(stop)); });
        return Results.Ok(responseStops);
    }

    public static async Task<IResult> GetPrivateStops(
        StopFunctions stops
    )
    {
        var allStops = await stops.GetAllStops();
        List<ResponseStopDto> responseStops = new();
        allStops
            .Where(stop => stop.StopGroupID == null).ToList()
            .ForEach(stop => { responseStops.Add(ResponseStopDto.FromStop(stop)); });
        return Results.Ok(responseStops);
    }

    [HttpGet("api/stops/{stopId}")]
    public static async Task<IResult> GetStopById(
        int stopId,
        StopFunctions stops
    )
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

    public static async Task<IResult> CreateStop(
        [FromBody] RequestStopDto stop,
        StopFunctions stops,
        StopGroupFunctions stopGroups,
        DivisionFunctions divisions
    )
    {
        try
        {
            StopGroup? stopGroup =
                stop.StopGroupID == null ? null : await stopGroups.GetStopGroupById(Convert.ToInt32(stop.StopGroupID));

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
                StopGroup = stopGroup
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

    public static async Task<IResult> UpdateStop(
        int stopId,
        [FromBody] RequestStopDto stop,
        StopGroupFunctions stopGroups,
        DivisionFunctions divisions,
        StopFunctions stops
    )
    {
        try
        {
            StopGroup? stopGroup =
                stop.StopGroupID == null ? null : await stopGroups.GetStopGroupById(Convert.ToInt32(stop.StopGroupID));
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
                return Results.BadRequest("Invalid Name");
            }

            if (stop.Description.Length > 255)
            {
                return Results.BadRequest("Invalid Description");
            }

            if (stop.RoomNr.Length > 5)
            {
                return Results.BadRequest("Invalid RoomNr");
            }

            var oldStop = await stops.GetStopById(stopId);

            await stops.UpdateStop(new Stop
            {
                StopID = stopId,
                Name = stop.Name,
                Description = stop.Description,
                RoomNr = stop.RoomNr,
                Division = await divisions.GetDivisionById(stop.DivisionID),
                StopGroupID = stop.StopGroupID,
                DivisionID = stop.DivisionID,
                StopGroup = stopGroup,
                StopOrder = oldStop.StopOrder
            });
            return Results.Ok(stop.StopGroupID);
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

    public static async Task<IResult> DeleteStop(
        int stopId,
        StopFunctions stops
    )
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

    public static async Task<IResult> GetStopsByGroupId(
        int groupId,
        StopFunctions stops,
        StopGroupFunctions stopGroups
    )
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

    public static async Task<IResult> UpdateOrder(
        RequestOrderDto order,
        StopFunctions stops
    )
    {
        try
        {
            for (var i = 0; i < order.Order.Length; i++)
            {
                var stop = await stops.GetStopById(order.Order[i]);
                stop.StopOrder = i;
                await stops.UpdateStop(stop);
            }

            return Results.Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound();
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }
}