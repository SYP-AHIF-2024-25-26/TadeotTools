using API.Dtos.RequestDtos;
using API.Dtos.ResponseDtos;
using API.RequestDto;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Endpoints;

public static class StopEndpoints
{
    public static void MapStopEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("v1");
        group.MapGet("api/stops", GetAllStops);
        group.MapGet("api/stops/private", GetPrivateStops);
        group.MapGet("api/stops/{stopId}", GetStopById);
        group.MapPost("api/stops", CreateStop);
        group.MapPut("api/stops/{stopId}", UpdateStop);
        group.MapDelete("api/stops/{stopId}", DeleteStop);
        group.MapGet("stops/groups/{groupId}", GetStopsByGroupId);
        group.MapPut("api/stops/order", UpdateOrder);
    }

    private static async Task<IResult> GetAllStops(
        StopFunctions stops
    )
    {
        var allStops = await stops.GetAllStops();
        List<ResponseStopDto> responseStops = new();
        allStops.ForEach(stop => { responseStops.Add(ResponseStopDto.FromStop(stop)); });
        return Results.Ok(responseStops);
    }

    private static async Task<IResult> GetPrivateStops(
        StopFunctions stops
    )
    {
        var allStops = await stops.GetAllStops();
        List<ResponseStopDto> responseStops = new();
        allStops
            // TODO: rewrite query. Assignment could also be null if they were not included in the database query
            .Where(stop => stop.StopGroupAssignments.Count == 0).ToList()
            .ForEach(stop => { responseStops.Add(ResponseStopDto.FromStop(stop)); });
        return Results.Ok(responseStops);
    }

    [HttpGet("api/stops/{stopId}")]
    private static async Task<IResult> GetStopById(
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

    private static async Task<IResult> CreateStop(
        [FromBody] RequestStopDto stop,
        StopFunctions stops,
        StopGroupFunctions stopGroups,
        DivisionFunctions divisions
    )
    {
        try
        {
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
                // TODO: handle relations to division and stopgroup properly
                //DivisionID = stop.DivisionID,
                //StopGroupID = stop.StopGroupID,
            };

            stopToAdd.Id = await stops.AddStop(stopToAdd);

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

    private static async Task<IResult> UpdateStop(
        int stopId,
        [FromBody] RequestStopDto stop,
        StopGroupFunctions stopGroups,
        DivisionFunctions divisions,
        StopFunctions stops
    )
    {
        try
        {
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

            // TODO: DONOT DO THIS!!!
            // Use db context change tracker to update an entity
            // fetch the object, update the properties, save changes
            var oldStop = await stops.GetStopById(stopId);

            await stops.UpdateStop(new Stop
            {
                Id = stopId,
                Name = stop.Name,
                Description = stop.Description,
                RoomNr = stop.RoomNr,
                //StopGroupID = stop.StopGroupID,
                //DivisionID = stop.DivisionID,
                //StopOrder = oldStop.StopOrder
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

    private static async Task<IResult> DeleteStop(
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

    private static async Task<IResult> GetStopsByGroupId(
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

    private static async Task<IResult> UpdateOrder(
        RequestOrderDto order,
        StopFunctions stops
    )
    {
        try
        {
            for (var i = 0; i < order.Order.Length; i++)
            {
                var stop = await stops.GetStopById(order.Order[i]);
                // TODO: Handle ranking correctly!
                //stop.StopOrder = i;
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