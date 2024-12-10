using Core.Entities;
using TadeoT.Database.Functions;

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
            StopID = stop.Id,
            Name = stop.Name,
            Description = stop.Description,
            RoomNr = stop.RoomNr,
            // TODO: Handle relations to StopGroup and Division correctly!
            StopGroupID = -1,
            DivisionID = -1
        };
    }
}