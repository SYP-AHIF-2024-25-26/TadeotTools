using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("v1/stops")]
public class StopsController : ControllerBase
{
    [HttpGet("{stopId}")]
    public IActionResult GetStopById(int stopId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Route("stats/{stopId}")]
    public IActionResult CreateStopStats(int stopId, [FromBody] object stats)
    {
        throw new NotImplementedException();
    }

    [HttpPut("stats/{stopId}")]
    public IActionResult UpdateStopStats(int stopId, [FromBody] object stats)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("stats/{stopId}")]
    public IActionResult DeleteStopStats(int stopId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult CreateStop([FromBody] object stop)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{stopId}")]
    public IActionResult UpdateStop(int stopId, [FromBody] object stop)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{stopId}")]
    public IActionResult DeleteStop(int stopId)
    {
        throw new NotImplementedException();
    }
}
