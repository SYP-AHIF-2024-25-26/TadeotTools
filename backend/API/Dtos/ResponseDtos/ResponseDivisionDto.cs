using TadeoT.Database.Model;

namespace API.Dtos.ResponseDtos;

public class ResponseDivisionDto
{
    public int DivisionID { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    
    public static ResponseDivisionDto FromDivision(Division division)
    {
        return new ResponseDivisionDto()
        {
            DivisionID = division.DivisionID,
            Name = division.Name,
            Color = division.Color
        };
    }
}