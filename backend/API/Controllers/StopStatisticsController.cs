using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Controllers;

[ApiController]
[Route("v1/stats")]
public class GroupStopsController : ControllerBase
{
    [HttpPost]
    [Route("{stopId}")]
    public IActionResult CreateStopStats(int stopId) {
        try {
            StopStatisticFunctions.GetInstance().AddStopStatistic(new StopStatistic() {
                IsDone = true,
                StopID = stopId,
                Stop = StopFunctions.GetInstance().GetStopById(stopId),
                Time = DateTime.Now,
            });
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{stopId}")]
    public IActionResult UpdateStopStats(int stopId, [FromBody] StopStatistic stats) {
        try {
            stats.StopID = stopId;
            StopStatisticFunctions.GetInstance().UpdateStopStatistic(stats);
            return Ok();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{stopId}")]
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
