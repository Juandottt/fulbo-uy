namespace FulboUY.Application.DTOs.Match;

public class TeamDto
{
    public int TeamNumber { get; set; }
    public IEnumerable<MatchParticipantDto> Players { get; set; } = [];
    public double AverageSkill { get; set; }
}
