﻿namespace TadeoT.Database.Model;

public class Stop
{
    public int StopID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string RoomNr { get; set; }
    public int StopOrder { get; set; } // defaults to 0 in database
    public int DivisionID { get; set; }
    public Division? Division { get; set; }
    public int? StopGroupID { get; set; }
    public StopGroup? StopGroup { get; set; }
}
