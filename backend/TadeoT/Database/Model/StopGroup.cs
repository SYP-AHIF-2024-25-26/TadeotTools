namespace TadeoT.Database.Model;

public class StopGroup
{
    public int StopGroupID { get; set; }
    public int StopGroupOrder { get; set; } // defaults to 0 in database
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsPublic { get; set; }
}
