namespace API.Dtos.ResponseDtos;

public class ResponseStopGroupDto
{
    public int StopGroupID { get; set; }
    public required String Name { get; set; }
    public required string Description { get; set; }
}