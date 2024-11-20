using API.RequestDtos;
using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

namespace API.Controllers;

[ApiController]
[Route("v1")]
public class DivisionsController(
    DivisionFunctions divisions
) : ControllerBase {
    [HttpGet("divisions")]
    public async Task<IResult> GetDivisions() {  
        try {
            return Results.Ok(await divisions.GetAllDivisions());
        }
        catch (Exception) {
            return Results.StatusCode(500);
        }
    }

    [HttpDelete("api/divisions/{divisionId}")]
    public async Task<IResult> DeleteDivisionById(int divisionId) {
        try
        {
            await divisions.GetDivisionById(divisionId);
            await divisions.DeleteDivisionById(divisionId);
            return Results.Ok();
        }
        catch (TadeoTNotFoundException) {
            return Results.NotFound("Could not find Division");
        }
        catch (TadeoTDatabaseException) {
            return Results.StatusCode(500);
        }
    }
    
    [HttpPost("api/divisions")]
    public async Task<IResult> CreateDivision([FromBody] RequestDivsionDto division) {
        try {
            if (division.Name.Length > 50) {
                return Results.BadRequest("Invalid Name");
            }
            if (division.Color.Length > 7) {
                return Results.BadRequest("Invalid Color");
            }
            var divisionId = await divisions.AddDivision(new Division {
                Name = division.Name,
                Color = division.Color
            });
            return Results.Ok(await divisions.GetDivisionById(divisionId));
        }
        catch (TadeoTDatabaseException) {
            return Results.StatusCode(500);
        }
    }
    
    [HttpPut("api/divisions/{divisionId}")] 
    public async Task<IResult> UpdateDivision(int divisionId, [FromBody] RequestDivsionDto division) {
        try {
            if (division.Name.Length > 50) {
                return Results.BadRequest("Invalid Name");
            }
            if (division.Color.Length > 7) {
                return Results.BadRequest("Invalid Color");
            }

            await divisions.GetDivisionById(divisionId);
            await divisions.UpdateDivision(new Division {
                DivisionID = divisionId,
                Name = division.Name,
                Color = division.Color
            });
            return Results.Ok();
        }
        catch (TadeoTNotFoundException) {
            return Results.NotFound("Could not find Division");
        }
        catch (TadeoTDatabaseException) {
            return Results.StatusCode(500);
        }
    }
}