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
    public async Task<IResult> GetGroups()
    {
        try
        {
            var allStopGroups = await stopGroups.GetAllStopGroups();
            return Results.Ok(allStopGroups
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
            return Results.StatusCode(500);
        }
    }

    [HttpGet("api/groups")]
    public async Task<IResult> GetGroupsApi()
    {
        try
        {
            return Results.Ok(await stopGroups.GetAllStopGroups());
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }
    

    [HttpPost("api/groups")]
    public async Task<IResult> CreateGroup([FromBody] RequestStopGroupDto group) {
        try {
            if (group.Name.Length > 50) {
                return Results.BadRequest("Invalid Name");
            }
            
            if (group.Description.Length > 255) {
                return Results.BadRequest("Invalid Description");
            }

            var stopGroupToAdd = new StopGroup {
                Name = group.Name,
                Description = group.Description,
                IsPublic = group.IsPublic
            };
            var stopGroupId = await stopGroups.AddStopGroup(stopGroupToAdd);
            stopGroupToAdd.StopGroupID = stopGroupId;
            return Results.Ok(stopGroupToAdd);
        }
        catch (TadeoTDatabaseException) {
            return Results.StatusCode(500);
        }
    }

    [HttpPut("api/groups/{groupId}")]
    public async Task<IResult> UpdateGroup(int groupId, [FromBody] RequestStopGroupDto group) {
        try
        {
            await stopGroups.GetStopGroupById(groupId);
            if (group.Name.Length > 50)
            {
                return Results.BadRequest("Invalid Name");
            }

            if (group.Description.Length > 255)
            {
                return Results.BadRequest("Invalid Description");
            }

            var stopGroup = new StopGroup
            {
                StopGroupID = groupId,
                Name = group.Name,
                Description = group.Description,
                IsPublic = group.IsPublic
            };

            await stopGroups.UpdateStopGroup(stopGroup);
            return Results.Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("StopGroup not found!");
        }
        catch (TadeoTDatabaseException) {
            return Results.StatusCode(500);
        }
    }

    [HttpDelete("api/groups/{groupId}")]
    public async Task<IResult> DeleteGroup(int groupId) {
        try
        {
            await stopGroups.GetStopGroupById(groupId);
            await stopGroups.DeleteStopGroupById(groupId);
            return Results.Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("StopGroup not found!");
        }
        catch (TadeoTDatabaseException) {
            return Results.StatusCode(500);
        }
    }
}