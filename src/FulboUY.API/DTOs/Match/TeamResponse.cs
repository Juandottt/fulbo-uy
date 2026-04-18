namespace FulboUY.API.DTOs.Match;

public class TeamResponse
{
    public int TeamNumber { get; set; }
    public IEnumerable<MatchParticipantResponse> Players { get; set; } = [];
    public double AverageSkill { get; set; }
}
