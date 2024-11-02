using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Controllers;

[ApiController]
[Route("v1/groups")]
public class StopGroupsController : ControllerBase {
    [HttpGet]
    public IActionResult GetGroups() {
        try {
            return Ok(StopGroupFunctions.GetInstance().GetAllStopGroups());
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "internal server error");
        }
    }

    [HttpGet("{groupId}")]
    public IActionResult GetGroupById(int groupId) {
        try {
            return Ok(StopGroupFunctions.GetInstance().GetStopsOfStopGroup(groupId));
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, $"Could not get stops with groupID {groupId}");
        }
    }

    [HttpPost]
    public IActionResult CreateGroup([FromBody] StopGroupDTO? group) {
        try {
            StopGroup stopGroupToAdd = new StopGroup {
                Name = group!.Name,
                Description = group.Description,
                Color = group.Color
            };
            int stopGroupId = StopGroupFunctions.GetInstance().AddStopGroup(stopGroupToAdd);
            stopGroupToAdd.StopGroupID = stopGroupId;
            return Ok(stopGroupToAdd);
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, $"Could not create group {group!.Name}");
        }
    }

    [HttpPut("{groupId}")]
    public IActionResult UpdateGroup([FromBody] StopGroup? group) {
        try {
            if (group == null) {
                return StatusCode(406, "Missing group data");
            }

            StopGroupFunctions.GetInstance().UpdateStopGroup(group);
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, $"Could not update group {group!.Name}");
        }
    }

    [HttpDelete("{groupId}")]
    public IActionResult DeleteGroup(int groupId) {
        try {
           StopGroupFunctions.GetInstance().DeleteStopGroupById(groupId);
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "No StopGroup with this id");
        }
    }
}