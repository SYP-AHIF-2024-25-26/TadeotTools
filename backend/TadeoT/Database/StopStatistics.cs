namespace TadeoT.Database;

public class StopStatistic {
    public required int StopStatisticID { get; set; }
    public required DateTime Time { get; set; }
    public required bool IsDone { get; set; }
    public required int StopID { get; set; }

    public required Stop Stop { get; set; }
}
