using Microsoft.AspNetCore.Mvc;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Controllers;

[ApiController]
[Route("v1/api/divisions")]
public class DivisionsController : ControllerBase {
    [HttpGet]
    public ActionResult GetDivisions() {
        try {
            return Ok(DivisionFunctions.GetInstance().GetAllDivisions());
        }
        catch (Exception) {
            return StatusCode(500, "Internal server error");
        }
    }

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
    
    /*
 PUT {{BASE_URL}}/v1/api/divisions/${divisionID}
   {
    divisionID
       name
       color
   }

   */
    
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
    }
}