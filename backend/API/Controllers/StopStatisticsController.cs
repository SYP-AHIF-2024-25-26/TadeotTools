using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("v1/api/stops/groups")]
public class GroupStopsController : ControllerBase
{
    [HttpGet("{groupId}")]
    public IActionResult GetStopsByGroupId(int groupId)
    {
        throw new NotImplementedException();
    }
}
