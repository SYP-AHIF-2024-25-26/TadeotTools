using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

namespace API.Controllers;

[ApiController]
[Route("v1/stops")]
public class StopsController : ControllerBase {
    [HttpGet("api")]
    public IActionResult GetAllStops() {
        try {
            return Ok(StopFunctions.GetInstance().GetAllStops());
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{stopId}")]
    public IActionResult GetStopById(int stopId) {
        try {
            return Ok(StopFunctions.GetInstance().GetStopById(stopId));
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(404, "Could not find Stop");
        }
    }

    [HttpPost("api")]
    public IActionResult CreateStop([FromBody] StopDto stop) {
        try {
            try {
                StopGroupFunctions.GetInstance().GetStopGroupById(stop.StopGroupID);
            }
            catch (TadeoTDatabaseException) {
                return StatusCode(404, "Could not find StopGroup");
            }

            if (stop.Name.Length > 50) return StatusCode(400, "Invalid Name");

            if (stop.Description.Length > 255) return StatusCode(400, "Invalid Description");

            if (stop.RoomNr.Length > 5) return StatusCode(400, "Invalid Room Number");


            var stopId = StopFunctions.GetInstance().AddStop(new Stop {
                Name = stop.Name,
                Description = stop.Description,
                RoomNr = stop.RoomNr,
                Division = DivisionFunctions.GetInstance().GetDivisionById(stop.DivisionID),
                StopGroup = StopGroupFunctions.GetInstance().GetStopGroupById(stop.StopGroupID)
            });
            return Ok(StopFunctions.GetInstance().GetStopById(stopId));
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Could not add Stop");
        }
    }

    [HttpPut("{stopId}")]
    public IActionResult UpdateStop(int stopId, [FromBody] StopDto stop) {
        try {
            if (stop.Name.Length > 50) return StatusCode(400, "Invalid Name");

            if (stop.Description.Length > 255) return StatusCode(400, "Invalid Description");

            if (stop.RoomNr.Length > 5) return StatusCode(400, "Invalid Room Number");

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
    }

    [HttpGet("groups/{groupId}")]
    public IActionResult GetStopsByGroupId(int groupId) {
        StopGroup? stopGroup = null;
        try {
            stopGroup = StopGroupFunctions.GetInstance().GetStopGroupById(groupId);
            var stops = StopGroupFunctions.GetInstance().GetStopsOfStopGroup(groupId);
            return Ok(stops);
        }
        catch (TadeoTDatabaseException) {
            if (stopGroup == null) return StatusCode(404, "Stopgroup not found");
            return StatusCode(500, "Cannot get Stops");
        }
    }
}