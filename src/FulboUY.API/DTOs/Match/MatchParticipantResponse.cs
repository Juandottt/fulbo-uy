namespace FulboUY.API.DTOs.Match;

public class MatchParticipantResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PlayerProfileId { get; set; }
    public required string PlayerName { get; set; }
    public int TeamNumber { get; set; }
    public bool HasPaid { get; set; }
    public DateTime JoinedAt { get; set; }
}
