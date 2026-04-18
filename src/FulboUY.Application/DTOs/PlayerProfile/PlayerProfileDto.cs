namespace FulboUY.Application.DTOs.PlayerProfile;

public class PlayerProfileDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public int Speed { get; set; }
    public int Defense { get; set; }
    public int Passing { get; set; }
    public int Shooting { get; set; }
    public double AverageSkill => (Speed + Defense + Passing + Shooting) / 4.0;
}
