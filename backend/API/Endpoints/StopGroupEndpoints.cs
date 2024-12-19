using Database.Entities;
using Database.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        group.MapPut("api/groups/order/{groupId}", UpdateOrder);
    }

    public record GetGroupsResponse(int Id, string Name, string Description, int Rank, int[] StopIds);

    // TODO: Put this in Functions File
    public static GetGroupsResponse[] GetAllGroups(TadeoTDbContext context, bool onlyPublic)
        => context.StopGroups
            .Where(g => onlyPublic ? g.IsPublic : true)
            .Select(g => new GetGroupsResponse(
                g.Id,
                g.Name,
                g.Description,
                g.Rank,
                context.StopGroupAssignments
                    .Where(a => a.StopGroupId == g.Id)
                    .OrderBy(a => a.Order)
                    .Select(a => a.StopId).ToArray()
            )).ToArray();

    public static async Task<IResult> GetGroups(TadeoTDbContext context)
    {
        return Results.Ok(GetAllGroups(context, true));
    }

    public static async Task<IResult> GetGroupsApi(TadeoTDbContext context)
    {
        return Results.Ok(GetAllGroups(context, false));
    }

    public record CreateGroupRequestDto(string Name, string Description, bool IsPublic);
    
    public static async Task<IResult> CreateGroup(TadeoTDbContext context, CreateGroupRequestDto dto)
    {
        var group = new StopGroup()
        {
            Name = dto.Name,
            Description = dto.Description,
            IsPublic = dto.IsPublic,
        };

        context.StopGroups.Add(group);
        await context.SaveChangesAsync();
        
        return Results.Ok(group);
    }

    
    public record UpdateGroupRequestDto(int Id, string Name, string Description, bool IsPublic, int[] StopIds);
    public static async Task<IResult> UpdateGroup(TadeoTDbContext context, UpdateGroupRequestDto dto)
    {
        var group = await context.StopGroups.FindAsync(dto.Id);
        if (group == null)
        {
            return Results.NotFound("Group not found");
        }
        
        group.Name = dto.Name;
        group.Description = dto.Description;
        group.IsPublic = dto.IsPublic;

        var assignments = dto.StopIds.Select((id, index) => new StopGroupAssignment()
        {
            StopGroupId = group.Id,
            StopGroup = group,
            StopId = id,
            Stop = context.Stops.Find(id),
            Order = index
        });
        group.StopAssignments.Clear();
        group.StopAssignments.AddRange(assignments);

        return Results.Ok();
    }

    public static async Task<IResult> DeleteGroup(TadeoTDbContext context, [FromRoute] int groupId)
    {
        var group = await context.StopGroups.FindAsync(groupId);
        if (group == null)
        {
            return Results.NotFound($"StopGroup with ID {groupId} not found");
        }

        context.StopGroups.Remove(group);
        await context.SaveChangesAsync();
        return Results.Ok();
    }

    public static async Task<IResult> UpdateOrder(TadeoTDbContext context, [FromRoute] int groupId, int[] order)
    {
        // TODO: Check if StopGroups exist
        var group = await context.StopGroups
            .Include(group => group.StopAssignments)
            .SingleOrDefaultAsync(group => group.Id == groupId);
        if (group == null)
        {
            return Results.NotFound($"StopGroup with ID {groupId} not found");
        }

        var assignments = order.Select((id, index) => new StopGroupAssignment()
        {
            StopGroupId = group.Id,
            StopGroup = group,
            StopId = id,
            Stop = context.Stops.Find(id),
            Order = index,
        });
        
        group.StopAssignments.Clear();
        group.StopAssignments.AddRange(assignments);

        await context.SaveChangesAsync();
        
        return Results.Ok();
    }
}