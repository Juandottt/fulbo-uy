using System.ComponentModel.DataAnnotations;

namespace FulboUY.API.DTOs.PlayerProfile;

public class UpdatePlayerProfileRequest
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [Range(1, 10)]
    public int? Speed { get; set; }

    [Range(1, 10)]
    public int? Defense { get; set; }

    [Range(1, 10)]
    public int? Passing { get; set; }

    [Range(1, 10)]
    public int? Shooting { get; set; }
}
