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
            StopGroupFunctions.GetInstance().AddStopGroup(new StopGroup {
                Name = group!.Name,
                Description = group.Description,
                Color = group.Color,
            });
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, $"Could not create group {group!.Name}");
        }
        catch (NullReferenceException) {
            return StatusCode(406, "Missing group data");
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
            try {
                StopGroupFunctions.GetInstance().GetStopGroupById(groupId);
            }
            catch (TadeoTDatabaseException) {
                return StatusCode(404, "No group found");
            }
            StopGroupFunctions.GetInstance().DeleteStopGroupById(groupId);
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Could not delete group");
        }
    }
}