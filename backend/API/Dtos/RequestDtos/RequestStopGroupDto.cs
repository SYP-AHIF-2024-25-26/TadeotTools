namespace API.Dtos.RequestDtos;

public class RequestStopGroupDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsPublic { get; set; }
}