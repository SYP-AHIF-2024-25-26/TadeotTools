using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class StopStatistic
{
    [Key]
    public int Id { get; set; }
    public required DateTime Time { get; set; }
    public required bool IsDone { get; set; }

    public required int StopId { get; set; }
    public required Stop? Stop { get; set; }
}
