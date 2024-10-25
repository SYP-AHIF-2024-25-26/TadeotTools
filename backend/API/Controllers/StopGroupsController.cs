using System.Text.RegularExpressions;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database.Functions;
using TadeoT.Database;

namespace API.Controllers;

[ApiController]
[Route("v1/groups")]
public class StopGroupsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetGroups() {
        try {
            return Ok(StopGroupFunctions.GetInstance().GetAllStopGroups());
        }
        catch (Exception e) {
            return StatusCode(500, "internal server error");
        }
    }

    [HttpGet("{groupId}")]
    public IActionResult GetGroupById(int groupId)
    {
        try {
            return Ok(StopGroupFunctions.GetInstance().GetStopsOfStopGroup(groupId));
        }
        catch (TadeoTDatabaseException e) {
            return StatusCode(500, $"Could not get stops with groupID {groupId}");
        }
    }

    [HttpPost]
    public IActionResult CreateGroup([FromBody] StopGroupDTO group) {
        StopGroupFunctions.GetInstance().AddStopGroup(new StopGroup {
            Name = group.Name,
            Description = group.Description,
            Color = group.Color,
        });
        throw new NotImplementedException();
    }

    [HttpPut("{groupId}")]
    public IActionResult UpdateGroup(int groupId, [FromBody] object group)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{groupId}")]
    public IActionResult DeleteGroup(int groupId)
    {
        throw new NotImplementedException();
    }
}
