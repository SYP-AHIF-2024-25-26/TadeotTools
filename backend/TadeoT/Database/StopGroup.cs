﻿namespace TadeoT.Database;
public class StopGroup {
    public int StopGroupID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Color { get; set; }

    public required ICollection<Stop> Stops { get; set; }
}
