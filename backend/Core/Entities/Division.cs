using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Division
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public byte[]? Image { get; set; }

    public List<Stop> Stops { get; set; } = new();
}
