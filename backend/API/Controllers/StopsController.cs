using API.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

namespace API.Controllers;

[ApiController]
[Route("v1/stops")]
public class StopsController(
    StopFunctions stops,
    StopGroupFunctions stopGroups,
    DivisionFunctions divisions
) : ControllerBase
{
    [HttpGet("api")]
    public async Task<IResult> GetAllStops()
    {
        var allStops = await stops.GetAllStops();
        List<ResponseStopDto> responseStops = new();
        allStops.ForEach(stop =>
        {
            responseStops.Add(ResponseStopDto.FromStop(stop));
        });
        return Results.Ok(responseStops);
    }

    [HttpGet("{stopId}")]
    public async Task<IResult> GetStopById(int stopId)
    {
        try
        {
            return Results.Ok(await stops.GetStopById(stopId));
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(404);
        }
    }

    [HttpPost("api")]
    public async Task<IResult> CreateStop([FromBody] StopDto stop)
    {
        try
        {
            try
            {
                await stopGroups.GetStopGroupById(stop.StopGroupID);
            }
            catch (TadeoTDatabaseException)
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


            var stopId = await stops.AddStop(new Stop
            {
                Name = stop.Name,
                Description = stop.Description,
                RoomNr = stop.RoomNr,
                Division = await divisions.GetDivisionById(stop.DivisionID),
                StopGroup = await stopGroups.GetStopGroupById(stop.StopGroupID)
            });
            return Results.Problem(detail: "Could not add Stop", statusCode: 500);
            return Results.Ok(stops.GetStopById(stopId));
        }
        catch (TadeoTDatabaseException)
        {
            return Results.Problem(detail: "Could not add Stop", statusCode: 500);
        }
    }
/*
    [HttpPut("{stopId}")]
    public IActionResult UpdateStop(int stopId, [FromBody] StopDto stop) {
        try {
            if (stop.Name.Length > 50) {
                return StatusCode(400, "Invalid Name");
            }

            if (stop.Description.Length > 255) {
                return StatusCode(400, "Invalid Description");
            }

            if (stop.RoomNr.Length > 5) {
                return StatusCode(400, "Invalid Room Number");
            }

            try {
                StopFunctions.GetInstance().GetStopById(stopId);
            }
            catch (TadeoTDatabaseException) {
                return StatusCode(404, "Stop not found");
            }

            StopFunctions.GetInstance().UpdateStop(new Stop {
                StopID = stopId,
                Name = stop.Name,
                Description = stop.Description,
                RoomNr = stop.RoomNr,
                Division = DivisionFunctions.GetInstance().GetDivisionById(stop.DivisionID),
                StopGroup = StopGroupFunctions.GetInstance().GetStopGroupById(stop.StopGroupID)
            });
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Could not update Stop");
        }
    }

    [HttpDelete("{stopId}")]
    public IActionResult DeleteStop(int stopId) {
        Stop? stopToUpdate = null;
        try {
            stopToUpdate = StopFunctions.GetInstance().GetStopById(stopId);
            StopFunctions.GetInstance().DeleteStopById(stopId);
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            if (stopToUpdate == null) return StatusCode(404, "Stop not found");

            return StatusCode(500, "Could not delete Stop");
        }
    }*/

    [HttpGet("groups/{groupId}")]
    public async Task<IResult> GetStopsByGroupId(int groupId) {
        try
        {
            await stopGroups.GetStopGroupById(groupId);
            List<ResponseStopDto> responseStops = new();
            var stopsByGroup = await stopGroups.GetStopsOfStopGroup(groupId);
            stopsByGroup.ForEach(stop =>
            {
                responseStops.Add(ResponseStopDto.FromStop(stop));
            });
            return Results.Ok(responseStops);
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("StopGroup not found");
        }
        catch (TadeoTDatabaseException) {
            return Results.StatusCode(500);
        }
    }
}