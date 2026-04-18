namespace FulboUY.Application.DTOs.Match;

public class MatchDto
{
    public Guid Id { get; set; }
    public required string Location { get; set; }
    public DateTime Date { get; set; }
    public decimal FieldCost { get; set; }
    public int MaxPlayers { get; set; }
    public required string Status { get; set; }
    public int ParticipantCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
