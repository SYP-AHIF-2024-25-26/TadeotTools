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
        StopStatisticFunctions.GetInstance().AddStopStatistic(new StopStatistic() {
            IsDone = true,
            StopID = stopId,
            Time = DateTime.Now,
            Stop = StopFunctions.GetInstance().GetStopById(stopId)
        });
    }

    [HttpPut("stats/{stopId}")]
    public IActionResult UpdateStopStats(int stopId, [FromBody] object stats) {
        throw new NotImplementedException();
    }

    [HttpDelete("stats/{stopId}")]
    public IActionResult DeleteStopStats(int stopId) {
        throw new NotImplementedException();
    }
}
