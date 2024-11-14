using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Controllers;

[ApiController]
[Route("v1/groups")]
public class StopGroupsController : ControllerBase {
    /*
    [HttpGet]
    public IActionResult GetGroups() {
        try {
            return Ok(StopGroupFunctions.GetInstance().GetAllStopGroups());
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "internal server error");
        }
    }

    [HttpGet("api")]
    public IActionResult GetGroupsApi() {
        return GetGroups();
    }

    [HttpGet("{groupId}")]
    public IActionResult GetGroupById(int groupId) {
        try {
            var stops = StopGroupFunctions.GetInstance().GetStopsOfStopGroup(groupId);

            if (stops.Count == 0) return StatusCode(404, $"No Stops found for StopGroup: {groupId}");

            return Ok(stops);
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("api")]
    public IActionResult CreateGroup([FromBody] StopGroupDto? group) {
        try {
            if (group == null) return StatusCode(400, "Missing Request Body");

            if (group.Description.Length > 255) return StatusCode(400, "Invalid Description");

            if (group.Color.Length > 7) return StatusCode(400, "Invalid Color");

            var stopGroupToAdd = new StopGroup {
                Name = group.Name,
                Description = group.Description
            };
            var stopGroupId = StopGroupFunctions.GetInstance().AddStopGroup(stopGroupToAdd);
            stopGroupToAdd.StopGroupID = stopGroupId;
            return Ok(stopGroupToAdd);
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, $"Could not create group {group!.Name}");
        }
    }

    [HttpPut("api/{groupId}")]
    public IActionResult UpdateGroup(int groupId, [FromBody] StopGroupDto? group) {
        try {
            if (group == null) return StatusCode(406, "Missing group data");


            if (group.Description.Length > 255) return StatusCode(400, "Invalid Description");

            if (group.Color.Length > 7) return StatusCode(400, "Invalid Color");

            var stopGroup = new StopGroup {
                StopGroupID = groupId,
                Name = group.Name,
                Description = group.Description
            };

            StopGroupFunctions.GetInstance().UpdateStopGroup(stopGroup);
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, $"Could not update group {group!.Name}");
        }
    }

    [HttpDelete("api/{groupId}")]
    public IActionResult DeleteGroup(int groupId) {
        try {
            StopGroupFunctions.GetInstance().DeleteStopGroupById(groupId);
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(404, "No StopGroup with this id");
        }
    }*/
}