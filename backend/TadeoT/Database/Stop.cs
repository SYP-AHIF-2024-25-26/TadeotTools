namespace TadeoT.Database;

public class Stop {
    public int StopID { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string RoomNr { get; set; }
    public int? StopGroupID { get; set; }

    public required StopGroup StopGroup { get; set; }

    public ICollection<StopStatistic> ?StopStatistics { get; set; }
}
