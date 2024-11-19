using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Controllers;

[ApiController]
[Route("v1/divisions")]
public class DivisionsController(
    DivisionFunctions divisions
) : ControllerBase {
    [HttpGet]
    public async Task<IResult> GetDivisions() {  
        try {
            return Results.Ok(await divisions.GetAllDivisions());
        }
        catch (Exception) {
            return Results.StatusCode(500);
        }
    }/*

    [HttpDelete("{divisionId}")]
    public ActionResult DeleteDivisionById(int divisionId) {
        try {
            DivisionFunctions.GetInstance().DeleteDivisionById(divisionId);
            return Ok();
        }
        catch (TadeoTNotFoundException) {
            return StatusCode(404, "Could not find Division");
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }
    }
    

    
    [HttpPost]
    public ActionResult CreateDivision([FromBody] DivisionDto division) {
        try {
            if (division.Name.Length > 50) {
                return StatusCode(400, "Invalid Name");
            }
            if (division.Color.Length > 7) {
                return StatusCode(400, "Invalid Color");
            }
            var divisionId = DivisionFunctions.GetInstance().AddDivision(new Division {
                Name = division.Name,
                Color = division.Color
            });
            return Ok(DivisionFunctions.GetInstance().GetDivisionById(divisionId));
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPut("{divisionId}")]
    public ActionResult UpdateDivision(int divisionId, [FromBody] DivisionDto division) {
        try {
            if (division.Name.Length > 50) {
                return StatusCode(400, "Invalid Name");
            }
            if (division.Color.Length > 7) {
                return StatusCode(400, "Invalid Color");
            }

            DivisionFunctions.GetInstance().GetDivisionById(divisionId);
            
            DivisionFunctions.GetInstance().UpdateDivision(new Division {
                DivisionID = divisionId,
                Name = division.Name,
                Color = division.Color
            });
            return Ok(DivisionFunctions.GetInstance().GetDivisionById(divisionId));
        }
        catch (TadeoTNotFoundException) {
            return StatusCode(404, "Could not find Division");
        }
        catch (TadeoTDatabaseException) {
            return StatusCode(500, "Internal server error");
        }
    }*/
}