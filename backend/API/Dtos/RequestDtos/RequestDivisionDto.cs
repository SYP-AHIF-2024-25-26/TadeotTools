namespace API.RequestDtos;

public class RequestDivisionDto
{
    public required string Name { get; set; }
    public required string Color { get; set; }
    public required byte[]? Image { get; set; }
}