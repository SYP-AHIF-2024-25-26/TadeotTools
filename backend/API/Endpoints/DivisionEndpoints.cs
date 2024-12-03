using API.Dtos.ResponseDtos;
using API.RequestDtos;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

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
        group.MapPut("api/divisions/{divisionId}/image", UpdateDivisionImage);
        group.MapGet("api/divisions/{divisionId}/image", GetImageByDivisionId);
    }
    
    public static async Task<IResult> GetDivisions(
        DivisionFunctions divisions
    )
    {
        try
        {
            var allDivisions = await divisions.GetAllDivisions();
            return Results.Ok(allDivisions.Select(division => ResponseDivisionDto.FromDivision(division)));
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

            var divisionId = await divisions.AddDivision(new Division
            {
                Name = division.Name,
                Color = division.Color,
                Image = null,
            });
            return Results.Ok(await divisions.GetDivisionById(divisionId));
        }
        catch (TadeoTDatabaseException)
        {
            return Results.StatusCode(500);
        }
    }
    
    [HttpPut("api/divisions/{divisionId}")]
    public static async Task<IResult> UpdateDivision(
        int divisionId,
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

            await divisions.GetDivisionById(divisionId);
            await divisions.UpdateDivision(new Division
            {
                DivisionID = divisionId,
                Name = division.Name,
                Color = division.Color,
                Image = null
            });
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
            await divisions.GetDivisionById(divisionId);
            await divisions.DeleteDivisionById(divisionId);
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

    public static async Task<IResult> GetImageByDivisionId(
        int divisionId,
        DivisionFunctions divisions
    )
    {
        try
        {
            var division = await divisions.GetDivisionById(divisionId);
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
        DivisionFunctions divisions
    )
    {
        try
        {
            if (image.Length == 0)
                return Results.BadRequest("No image uploaded.");

            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);

            Division division = await divisions.GetDivisionById(divisionId);
            await divisions.UpdateDivision(new Division
            {
                DivisionID = divisionId,
                Name = division.Name,
                Color = division.Color,
                Image = memoryStream.ToArray()
            });
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
}