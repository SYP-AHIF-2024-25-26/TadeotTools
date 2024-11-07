namespace TadeoT.Database;

public class Stop {
    public int StopID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string RoomNr { get; set; }
    public int? StopGroupID { get; set; }

    public StopGroup? StopGroup { get; set; }
}
