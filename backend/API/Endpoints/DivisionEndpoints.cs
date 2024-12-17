using API.Dtos.ResponseDtos;
using API.Dtos.RequestDtos;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Repository;
using Database.Repository.Functions;

namespace API.Endpoints;

public static class DivisionEndpoints
{
    public static void MapDivisionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("v1");
        group.MapGet("divisions", GetDivisions);
        group.MapDelete("api/divisions/{divisionId}", DeleteDivisionById);
        group.MapPost("api/divisions", CreateDivision);
        group.MapPut("api/divisions/{divisionId}", UpdateDivision);
        group.MapPut("api/divisions/{divisionId}/image", UpdateDivisionImage).DisableAntiforgery();
        group.MapGet("divisions/{divisionId}/image", GetImageByDivisionId).DisableAntiforgery();
    }
    
    public static async Task<IResult> GetDivisions()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> CreateDivision()
    {
        throw new NotImplementedException();
    }
    
    public record UpdateDivisionDto(string Name, string Color);
    
    public static async Task<IResult> UpdateDivision()
    {
        throw new NotImplementedException();
    }
    
    public static async Task<IResult> DeleteDivisionById()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> GetImageByDivisionId()
    {
        throw new NotImplementedException();
    }
    
    public static async Task<IResult> UpdateDivisionImage()
    {
        throw new NotImplementedException();
    }
}