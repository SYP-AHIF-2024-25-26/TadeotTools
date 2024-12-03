using API.Dtos.RequestDtos;
using API.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

namespace API.Controllers;

[ApiController]
[Route("v1")]
public class StopGroupsController(
    StopFunctions stops,
    StopGroupFunctions stopGroups,
    DivisionFunctions divisions
) : ControllerBase
{
    [HttpGet("groups")]
    public async Task<IActionResult> GetGroups()
    {
        try
        {
            var allStopGroups = await stopGroups.GetAllStopGroups();
            return Ok(allStopGroups
                .Where(stopGroup => stopGroup.IsPublic)
                .Select(stopGroup => new ResponseStopGroupDto()
                {
                    StopGroupID = stopGroup.StopGroupID,
                    Name = stopGroup.Name,
                    Description = stopGroup.Description,
                })
                .ToList());
        }
        catch (TadeoTDatabaseException)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("api/groups")]
    public async Task<IActionResult> GetGroupsApi()
    {
        try
        {
            return Ok(await stopGroups.GetAllStopGroups());
        }
        catch (TadeoTDatabaseException)
        {
            return StatusCode(500);
        }
    }
    

    [HttpPost("api/groups")]
    public async Task<IActionResult> CreateGroup([FromBody] RequestStopGroupDto group) {
        try {
            if (group.Name.Length > 50) {
                return BadRequest("Invalid Name");
            }
            
            if (group.Description.Length > 255) {
                return BadRequest("Invalid Description");
            }

            var stopGroupToAdd = new StopGroup {
                Name = group.Name,
                Description = group.Description,
                IsPublic = group.IsPublic
            };
            var stopGroupId = await stopGroups.AddStopGroup(stopGroupToAdd);
            stopGroupToAdd.StopGroupID = stopGroupId;
            return Ok(stopGroupToAdd);
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500);
        }
    }

    [HttpPut("api/groups/{groupId}")]
    public async Task<IActionResult> UpdateGroup(int groupId, [FromBody] RequestStopGroupDto group) {
        try
        {
            var oldStopGroup = await stopGroups.GetStopGroupById(groupId);
            if (group.Name.Length > 50)
            {
                return BadRequest("Invalid Name");
            }

            if (group.Description.Length > 255)
            {
                return BadRequest("Invalid Description");
            }

            var stopGroup = new StopGroup
            {
                StopGroupID = groupId,
                Name = group.Name,
                Description = group.Description,
                IsPublic = group.IsPublic,
                StopGroupOrder = oldStopGroup.StopGroupOrder
            };

            await stopGroups.UpdateStopGroup(stopGroup);
            return Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound("StopGroup not found!");
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500);
        }
    }

    [HttpDelete("api/groups/{groupId}")]
    public async Task<IActionResult> DeleteGroup(int groupId) {
        try
        {
            await stopGroups.GetStopGroupById(groupId);
            await stopGroups.DeleteStopGroupById(groupId);
            return Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound("StopGroup not found!");
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500);
        }
    }
    
    [HttpPut("api/groups/order")]
    public async Task<IActionResult> UpdateOrder(RequestOrderDto order) {
        try
        {
            for (var i = 0; i < order.Order.Length; i++)
            {
                var stopGroup = await stopGroups.GetStopGroupById(order.Order[i]);
                stopGroup.StopGroupOrder = i;
                await stopGroups.UpdateStopGroup(stopGroup);
            }
            return Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound();
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500);
        }
    }
}