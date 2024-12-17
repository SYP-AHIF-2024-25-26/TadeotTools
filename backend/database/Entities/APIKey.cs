using System.ComponentModel.DataAnnotations;

namespace Core.Entities;
public class APIKey
{
    public required string APIKeyValue { get; set; }
}
