namespace FulboUY.Application.DTOs.Match;

public class TeamsResultDto
{
    public TeamDto Team1 { get; set; } = new();
    public TeamDto Team2 { get; set; } = new();
    public double SkillDifference { get; set; }
}
