using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Controllers;

[ApiController]
[Route("v1/api/stops/")]
public class GroupStopsController : ControllerBase
{
    [HttpPost]
    [Route("stats/{stopId}")]
    public IActionResult CreateStopStats(int stopId) {
        try {
            StopStatisticFunctions.GetInstance().AddStopStatistic(new StopStatistic() {
                IsDone = true,
                StopID = stopId,
                Time = DateTime.Now,
                Stop = StopFunctions.GetInstance().GetStopById(stopId)
            });
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }

    }

    [HttpPut("stats/{stopId}")]
    public IActionResult UpdateStopStats(int stopId, [FromBody] StopStatistic stats) {
        try {
            StopStatisticFunctions.GetInstance().UpdateStopStatistic(stats);
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("stats/{stopId}")]
    public IActionResult DeleteStopStats(int stopId) {
        try {
            StopStatisticFunctions.GetInstance().DeleteStopStopStatisticById(stopId);
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }
    }
}
