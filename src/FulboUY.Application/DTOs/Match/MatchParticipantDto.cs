namespace FulboUY.Application.DTOs.Match;

public class MatchParticipantDto
{
    public Guid Id { get; set; }
    public Guid MatchId { get; set; }
    public Guid UserId { get; set; }
    public Guid PlayerProfileId { get; set; }
    public required string PlayerName { get; set; }
    public int TeamNumber { get; set; }
    public bool HasPaid { get; set; }
    public DateTime JoinedAt { get; set; }
}
