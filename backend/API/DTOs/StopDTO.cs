using TadeoT.Database;

namespace API.DTOs;

public class StopDTO {
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string RoomNr { get; set; }
    public int? StopGroupID { get; set; }

    public required StopGroup StopGroup { get; set; }
}