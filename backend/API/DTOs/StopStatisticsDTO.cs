using TadeoT.Database;

namespace API.DTOs;

public class StopStatisticsDTO {
    public required DateTime Time { get; set; }
    public required bool IsDone { get; set; }
    public required int StopID { get; set; }

    public required Stop Stop { get; set; }
}