namespace TadeoT.Database;

public enum StopGroupName {
    Informatik,
    Medientechnik,
    Elektronik,
    Medizintechnik,
    Neutral
}

public class StopGroup {
    public int StopGroupID { get; set; }
    public required StopGroupName Name { get; set; }
    public required string Description { get; set; }
    public required string Color { get; set; }
}
