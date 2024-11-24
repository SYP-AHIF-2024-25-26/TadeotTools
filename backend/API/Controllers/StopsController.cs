using API.Dtos.RequestDtos;
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
    public async Task<IActionResult> GetAllStops()
    {
        var allStops = await stops.GetAllStops();
        List<ResponseStopDto> responseStops = new();
        allStops.ForEach(stop => { responseStops.Add(ResponseStopDto.FromStop(stop)); });
        return Ok(responseStops);
    }
    
    [HttpGet("api/stops/private")]
    public async Task<IActionResult> GetPrivateStops()
    {
        var allStops = await stops.GetAllStops();
        List<ResponseStopDto> responseStops = new();
        allStops
            .Where(stop => stop.StopGroupID == null).ToList()
            .ForEach(stop => { responseStops.Add(ResponseStopDto.FromStop(stop)); });
        return Ok(responseStops);
    }

    [HttpGet("api/stops/{stopId}")]
    public async Task<IActionResult> GetStopById(int stopId)
    {
        try
        {
            var stop = await stops.GetStopById(stopId);
            return Ok(ResponseStopDto.FromStop(stop));
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound("Stop not found");
        }
    }

    [HttpPost("api/stops")]
    public async Task<IActionResult> CreateStop([FromBody] RequestStopDto stop)
    {
        try
        {
            StopGroup? stopGroup = null;
            try
            {
                stopGroup = await stopGroups.GetStopGroupById(stop.StopGroupID);
            }
            catch (TadeoTNotFoundException)
            {
            }

            if (stop.Name.Length > 50)
            {
                return BadRequest("Invalid Name");
            }

            if (stop.Description.Length > 255)
            {
                return BadRequest("Invalid Description");
            }

            if (stop.RoomNr.Length > 5)
            {
                return BadRequest("Invalid Room Number");
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

            return Ok(ResponseStopDto.FromStop(stopToAdd));
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound("Division not found");
        }
        catch (TadeoTDatabaseException)
        {
            return StatusCode(500);
        }
    }

    [HttpPut("api/stops/{stopId}")]
    public async Task<IActionResult> UpdateStop(int stopId, [FromBody] RequestStopDto stop)
    {
        try
        {
            try
            {
                await stopGroups.GetStopGroupById(stop.StopGroupID);
            }
            catch (TadeoTNotFoundException)
            {
                return NotFound("StopGroup not found");
            }

            try
            {
                await divisions.GetDivisionById(stop.DivisionID);
            }
            catch (TadeoTNotFoundException)
            {
                return NotFound("Division not found");
            }

            if (stop.Name.Length > 50)
            {
                return NotFound("Invalid Name");
            }

            if (stop.Description.Length > 255)
            {
                return NotFound("Invalid Description");
            }

            if (stop.RoomNr.Length > 5)
            {
                return NotFound("Invalid RoomNr");
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
            return Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound("Stop not found");
        }
        catch (TadeoTDatabaseException)
        {
            return StatusCode(500);
        }
    }

    [HttpDelete("api/stops/{stopId}")]
    public async Task<IActionResult> DeleteStop(int stopId)
    {
        try
        {
            await stops.GetStopById(stopId);
            await stops.DeleteStopById(stopId);
            return Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound("Stop not found");
        }
        catch (TadeoTDatabaseException)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("stops/groups/{groupId}")]
    public async Task<IActionResult> GetStopsByGroupId(int groupId)
    {
        try
        {
            await stopGroups.GetStopGroupById(groupId);
            List<ResponseStopDto> responseStops = new();
            var stopsByGroup = await stopGroups.GetStopsOfStopGroup(groupId);
            stopsByGroup.ForEach(stop => { responseStops.Add(ResponseStopDto.FromStop(stop)); });
            return Ok(responseStops);
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound("StopGroup not found");
        }
        catch (TadeoTDatabaseException)
        {
            return StatusCode(500);
        }
    }
    
    [HttpPut("api/stops/order")]
    public async Task<IActionResult> UpdateOrder(RequestOrderDto order) {
        try
        {
            for (var i = 0; i < order.Order.Length; i++)
            {
                var stop = await stops.GetStopById(order.Order[i]);
                stop.StopOrder = i;
                await stops.UpdateStop(stop);
            }
            return Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500);
        }
    }
}