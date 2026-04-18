namespace FulboUY.API.DTOs.PlayerProfile;

public class PlayerProfileResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public int Speed { get; set; }
    public int Defense { get; set; }
    public int Passing { get; set; }
    public int Shooting { get; set; }
    public double AverageSkill { get; set; }
}
