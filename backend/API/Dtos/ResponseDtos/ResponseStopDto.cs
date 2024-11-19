using TadeoT.Database.Functions;
using TadeoT.Database.Model;

namespace API.Dtos.ResponseDtos;

public class ResponseStopDto
{
    public int StopID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string RoomNr { get; set; }
    public int? StopGroupID { get; set; }
    public int DivisionID { get; set; }
    
    public static ResponseStopDto FromStop(Stop stop)
    {
        return new ResponseStopDto()
        {
            StopID = stop.StopID,
            Name = stop.Name,
            Description = stop.Description,
            RoomNr = stop.RoomNr,
            StopGroupID = stop.StopGroupID,
            DivisionID = stop.DivisionID
        };
    }
}