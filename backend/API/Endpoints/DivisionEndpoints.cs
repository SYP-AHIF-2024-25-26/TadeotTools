using API.Dtos.ResponseDtos;
using API.RequestDtos;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;

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
    
    public static async Task<IResult> GetDivisions(
        DivisionFunctions divisions
    )
    {
        try
        {
            var allDivisions = await divisions.GetAllDivisionsWithoutImageAsync();
            return Results.Ok(allDivisions);
        }
        catch (Exception)
        {
            return Results.StatusCode(500);
        }
    }

    public static async Task<IResult> CreateDivision(
        [FromBody] RequestDivisionDto division,
        DivisionFunctions divisions
    )
    {
        try
        {
            if (division.Name.Length > 50)
            {
                return Results.BadRequest("Invalid Name");
            }

            if (division.Color.Length > 7)
            {
                return Results.BadRequest("Invalid Color");
            }

            var dbDivision = new Division
            {
                Name = division.Name,
                Color = division.Color,
                Image = null,
            };
            await divisions.AddDivision(dbDivision);
            return Results.Ok(dbDivision);
        }
        catch (TadeoTDatabaseException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
    
    public record UpdateDivisionDto(string Name, string Color);
    [HttpPut("api/divisions/{divisionId}")]
    public static async Task<IResult> UpdateDivision(
        int divisionId,
        [FromBody] UpdateDivisionDto division,
        DivisionFunctions divisions,
        TadeoTDbContext context 
    )
    {
        try
        {
            if (division.Name.Length > 50)
            {
                return Results.BadRequest("Invalid Name");
            }

            if (division.Color.Length > 7)
            {
                return Results.BadRequest("Invalid Color");
            }
            var divisionDbEntity = await context.Divisions.FindAsync(divisionId);
            if (divisionDbEntity == null)
            {
                return Results.NotFound("Could not find Division");
            }
            divisionDbEntity.Name = division.Name;
            divisionDbEntity.Color = division.Color;
            await context.SaveChangesAsync();

            return Results.Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("Could not find Division");
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }
    
    public static async Task<IResult> DeleteDivisionById(
        int divisionId,
        DivisionFunctions divisions
    )
    {
        try
        {
            await divisions.DeleteDivisionById(divisionId);
            return Results.Ok();
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("Could not find Division");
        }
        catch (TadeoTDatabaseException e)
        {
            return Results.BadRequest(e.Message);
        }
    }

    public static async Task<IResult> GetImageByDivisionId(
        int divisionId,
        DivisionFunctions divisions
    )
    {
        try
        {
            var division = await divisions.GetDivisionById(divisionId);
            if (division == null)
            {
                return Results.NotFound("Could not find Division");
            }
            if (division.Image == null)
            {
                return Results.NoContent();
            }

            return Results.File(division.Image, "image/jpeg");
        }
        catch (TadeoTNotFoundException)
        {
            return Results.NotFound("Could not find Division");
        }
    }
    
    [HttpPut("api/divisions/{divisionId}/image")]
    public static async Task<IResult> UpdateDivisionImage(
        int divisionId,
        IFormFile image,
        DivisionFunctions divisions,
        TadeoTDbContext context
    )
    {
        try
        {
            if (image.Length == 0)
                return Results.BadRequest("No image uploaded.");

            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);

            Division? division = await divisions.GetDivisionById(divisionId);
            if (division == null)
            {
                return Results.NotFound("Could not find Division");
            }
            division.Image = memoryStream.ToArray();
            await context.SaveChangesAsync();

            return Results.Ok();
        }

        catch (DbUpdateException dbEx)
        {
            var message = dbEx.InnerException?.Message ?? dbEx.Message;
            return Results.BadRequest(message);
        }
    }
}