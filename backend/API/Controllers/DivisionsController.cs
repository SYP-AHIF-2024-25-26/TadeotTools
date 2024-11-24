using API.Dtos.ResponseDtos;
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
    public async Task<IActionResult> GetDivisions() {  
        try
        {
            var allDivisions = await divisions.GetAllDivisions();
            return Ok(allDivisions.Select(division => ResponseDivisionDto.FromDivision(division)));
        }
        catch (Exception) {
            return StatusCode(500, "Internal server error!");
        }
    }

    [HttpDelete("api/divisions/{divisionId}")]
    public async Task<IActionResult> DeleteDivisionById(int divisionId) {
        try
        {
            await divisions.GetDivisionById(divisionId);
            await divisions.DeleteDivisionById(divisionId);
            return Ok();
        }
        catch (TadeoTNotFoundException) {
            return NotFound("Could not find Division");
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "internal server error!");
        }
    }
    
    [HttpPost("api/divisions")]
    public async Task<IActionResult> CreateDivision([FromBody] RequestDivsionDto division) {
        try {
            if (division.Name.Length > 50) {
                return BadRequest("Invalid Name");
            }
            if (division.Color.Length > 7) {
                return BadRequest("Invalid Color");
            }
            var divisionId = await divisions.AddDivision(new Division {
                Name = division.Name,
                Color = division.Color,
                Image = division.Image
            });
            return Ok(await divisions.GetDivisionById(divisionId));
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "internal server error!");
        }
    }
    
    [HttpPut("api/divisions/{divisionId}")] 
    public async Task<IActionResult> UpdateDivision(int divisionId, [FromBody] RequestDivsionDto division) {
        try {
            if (division.Name.Length > 50) {
                return BadRequest("Invalid Name");
            }
            if (division.Color.Length > 7) {
                return BadRequest("Invalid Color");
            }

            await divisions.GetDivisionById(divisionId);
            await divisions.UpdateDivision(new Division {
                DivisionID = divisionId,
                Name = division.Name,
                Color = division.Color,
                Image = division.Image
            });
            return Ok();
        }
        catch (TadeoTNotFoundException) {
            return NotFound("Could not find Division");
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "internal server error!");
        }
    }

    [HttpGet("divisions/{divisionId}")]
    public async Task<IActionResult> GetImageByDivisionId(int divisionId)
    {
        try
        {
            var division = await divisions.GetDivisionById(divisionId);
            if (division.Image == null)
            {
                return NoContent();
            }
            return File(division.Image, "image/jpeg");
        }
        catch (TadeoTNotFoundException)
        {
            return NotFound("Could not find Division");
        }
    }
}