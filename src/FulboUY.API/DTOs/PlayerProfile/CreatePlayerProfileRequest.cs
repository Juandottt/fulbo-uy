using System.ComponentModel.DataAnnotations;

namespace FulboUY.API.DTOs.PlayerProfile;

public class CreatePlayerProfileRequest
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Range(1, 10)]
    public int Speed { get; set; } = 5;

    [Range(1, 10)]
    public int Defense { get; set; } = 5;

    [Range(1, 10)]
    public int Passing { get; set; } = 5;

    [Range(1, 10)]
    public int Shooting { get; set; } = 5;
}
