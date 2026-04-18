namespace FulboUY.API.DTOs.Match;

public class TeamsResultResponse
{
    public TeamResponse Team1 { get; set; } = new();
    public TeamResponse Team2 { get; set; } = new();
    public double SkillDifference { get; set; }
}
