using API.Dtos.RequestDtos;
using API.Dtos.ResponseDtos;
using API.RequestDto;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Endpoints;

public static class StopEndpoints
{
    public static void MapStopEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("v1");
        group.MapGet("api/stops", GetAllStops);
        group.MapGet("api/stops/private", GetPrivateStops);
        group.MapPost("api/stops", CreateStop);
        group.MapPut("api/stops/{stopId}", UpdateStop);
        group.MapDelete("api/stops/{stopId}", DeleteStop);
        group.MapGet("stops/groups/{groupId}", GetStopsByGroupId);
        group.MapPut("api/stops/order", UpdateOrder);
    }

    private static async Task<IResult> GetAllStops()
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> GetPrivateStops()
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> CreateStop()
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> UpdateStop()
    {
        throw new NotImplementedException();
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
}