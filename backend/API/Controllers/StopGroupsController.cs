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
        return Ok();
    }

    [HttpGet("{groupId}")]
    public IActionResult GetGroupById(int groupId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult CreateGroup([FromBody] object group)
    {
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
