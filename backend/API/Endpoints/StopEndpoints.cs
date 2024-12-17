using API.Dtos.RequestDtos;
using API.Dtos.ResponseDtos;
using API.RequestDto;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Endpoints;

public static class StopEndpoints
{
    public static void MapStopEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("v1");
        group.MapGet("api/stops", GetAllStops);
        group.MapPost("api/stops", CreateStop);
        group.MapPut("api/stops", UpdateStop);
        group.MapDelete("api/stops/{stopId}", DeleteStop);
        group.MapGet("stops/groups/{groupId}", GetStopsByGroupId);
        group.MapPut("api/stops/order", UpdateOrder);
    }

    private static async Task<IResult> GetAllStops(TadeoTDbContext context)
    {
        return Results.Ok(context.Stops
            .Select(stop => new StopResponseDto(
                    stop.Id,
                    stop.Name,
                    stop.Description,
                    stop.RoomNr,
                    stop.Divisions
                        .Select(d => d.Id)
                        .ToArray(),
                    stop.StopGroupAssignments
                        .Select(a => a.StopGroupId)
                        .ToArray()
                )
            )
        );
    }


    private static async Task<IResult> CreateStop(TadeoTDbContext context, CreateStopRequestDto createStopDto)
    {
        foreach (var divisionId in createStopDto.DivisionIds)
        {
            var division = await context.Divisions.FindAsync(divisionId);
            if (division == null)
            {
                return Results.NotFound($"Division with ID {divisionId} not found");
            }
        }

        var stop = new Stop()
        {
            Name = createStopDto.Name,
            Description = createStopDto.Description,
            RoomNr = createStopDto.RoomNr,
            Divisions = context.Divisions
                .Where(d => createStopDto.DivisionIds.Contains(d.Id))
                .ToList(),
        };
        context.Stops.Add(stop);

        var groupAssignments = createStopDto.StopGroupIds
            .Select(g =>
                new StopGroupAssignment()
                {
                    StopGroupId = g,
                    StopGroup = context.StopGroups.Find(g),
                    StopId = stop.Id,
                    Stop = stop,
                    Order = 0
                })
            .Where(g => g.StopGroup != null)
            .ToList();
        if (groupAssignments.Count != createStopDto.StopGroupIds.Length)
        {
            return Results.NotFound("StopGroupId not found");
        }

        stop.StopGroupAssignments = groupAssignments;

        await context.SaveChangesAsync();
        return Results.Ok(new StopResponseDto(stop.Id, stop.Name, stop.Description, stop.RoomNr,
            createStopDto.DivisionIds, createStopDto.StopGroupIds));
    }

    // Update name, description, roomNr, divisions, stopgroups
    private static async Task<IResult> UpdateStop(TadeoTDbContext context, UpdateStopRequestDto updateStopDto)
    {
        if (updateStopDto.Name.Length == 50)
        {
            return Results.BadRequest("Name must be 50 characters or less.");
        }

        if (updateStopDto.Description.Length == 255)
        {
            return Results.BadRequest("Description must be 50 characters or less.");
        }

        if (updateStopDto.RoomNr.Length == 50)
        {
            return Results.BadRequest("RoomNr must be 50 characters or less.");
        }

        await context
            .Stops
            .Where(s => s.Id == updateStopDto.Id)
            .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.Name, updateStopDto.Name)
                    .SetProperty(s => s.Description, updateStopDto.Description)
                    .SetProperty(s => s.RoomNr, updateStopDto.RoomNr)
                //.SetProperty(s => s.Divisions, context.Divisions.Where(di => updateStopDto.DivisionIds.Contains(di.Id)).ToList())
            );
        await context.SaveChangesAsync();
        return Results.Ok();
    }

    private static async Task<IResult> DeleteStop()
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> GetStopsByGroupId()
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> UpdateOrder()
    {
        throw new NotImplementedException();
    }


    public record UpdateStopRequestDto(
        int Id,
        string Name,
        string Description,
        string RoomNr,
        int[] DivisionIds,
        int[] StopGroupIds
    );

    public record CreateStopRequestDto(
        string Name,
        string Description,
        string RoomNr,
        int[] DivisionIds,
        int[] StopGroupIds
    );

    public record StopResponseDto(
        int StopId,
        string Name,
        string Description,
        string RoomNr,
        int[] DivisionIds,
        int[] StopGroupIds
    );
}