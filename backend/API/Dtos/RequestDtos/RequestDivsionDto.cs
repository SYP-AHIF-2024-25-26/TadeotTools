namespace API.RequestDtos;

public class RequestDivsionDto
{
    public required string Name { get; set; }
    public required string Color { get; set; }
    public required byte[]? Image { get; set; }
}