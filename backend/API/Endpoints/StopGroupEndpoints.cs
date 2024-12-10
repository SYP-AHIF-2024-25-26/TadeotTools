using System.Text.RegularExpressions;
using API.Dtos.RequestDtos;
using API.Dtos.ResponseDtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Endpoints;

public static class StopGroupEndpoints
{
    public static void MapStopGroupEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("v1");
        group.MapGet("groups", GetGroups);
        group.MapGet("api/groups", GetGroupsApi);
        group.MapPost("api/groups", CreateGroup);
        group.MapPut("api/groups/{groupId}", UpdateGroup);
        group.MapDelete("api/groups/{groupId}", DeleteGroup);
        group.MapPut("api/groups/order", UpdateOrder);
    }

    public static async Task<IResult> GetGroups(
        StopGroupFunctions stopGroups
    )
    {
        try
        {
            var allStopGroups = await stopGroups.GetAllStopGroups();
            return Results.Ok(allStopGroups
                .Where(stopGroup => stopGroup.IsPublic)
                .Select(stopGroup => new ResponseStopGroupDto()
                {
                    StopGroupID = stopGroup.Id,
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

    public static async Task<IResult> GetGroupsApi(
        StopGroupFunctions stopGroups
    )
    {
        try
        {
            var allStopGroups = await stopGroups.GetAllStopGroups();
            return Results.Ok(allStopGroups.Select(stopGroup => new ResponseApiStopGroupDto()
            {
                StopGroupID = stopGroup.Id,
                Name = stopGroup.Name,
                Description = stopGroup.Description,
                IsPublic = stopGroup.IsPublic
            }));
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }

    public static async Task<IResult> CreateGroup(
        [FromBody] RequestStopGroupDto group,
        StopGroupFunctions stopGroups
    )
    {
        try
        {
            if (group.Name.Length > 50)
            {
                return Results.BadRequest("Invalid Name");
            }

            if (group.Description.Length > 255)
            {
                return Results.BadRequest("Invalid Description");
            }

            var stopGroupToAdd = new StopGroup
            {
                Name = group.Name,
                Description = group.Description,
                IsPublic = group.IsPublic
            };
            var stopGroupId = await stopGroups.AddStopGroup(stopGroupToAdd);
            stopGroupToAdd.Id = stopGroupId;
            return Results.Ok(stopGroupToAdd);
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }

    public static async Task<IResult> UpdateGroup(
        int groupId,
        [FromBody] RequestStopGroupDto group,
        StopGroupFunctions stopGroups
    )
    {
        try
        {
            var oldStopGroup = await stopGroups.GetStopGroupById(groupId);
            if (group.Name.Length > 50)
            {
                return Results.BadRequest("Invalid Name");
            }

            if (group.Description.Length > 255)
            {
                return Results.BadRequest("Invalid Description");
            }

            // TODO: NO! This is not how you update a StopGroup
            var stopGroup = new StopGroup
            {
                Id = groupId,
                Name = group.Name,
                Description = group.Description,
                IsPublic = group.IsPublic,
                //StopGroupOrder = oldStopGroup.StopGroupOrder
            };

            await stopGroups.UpdateStopGroup(stopGroup);
            return Results.Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("StopGroup not found!");
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }

    public static async Task<IResult> DeleteGroup(
        int groupId,
        StopGroupFunctions stopGroups
    )
    {
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
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }

    public static async Task<IResult> UpdateOrder(
        RequestOrderDto order,
        StopGroupFunctions stopGroups
    )
    {
        try
        {
            for (var i = 0; i < order.Order.Length; i++)
            {
                var stopGroup = await stopGroups.GetStopGroupById(order.Order[i]);
                // NOOOO This is not how you update the order of a StopGroup
                //stopGroup.StopGroupOrder = i;
                await stopGroups.UpdateStopGroup(stopGroup);
            }

            return Results.Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound();
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }
}