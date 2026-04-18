using FulboUY.Domain.Enums;

namespace FulboUY.Domain.Entities;

public class Match
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Location { get; set; }
    public DateTime Date { get; set; }
    public decimal FieldCost { get; set; }
    public int MaxPlayers { get; set; }
    public MatchStatus Status { get; set; } = MatchStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navegación
    public ICollection<MatchParticipant> Participants { get; set; } = [];
    public ICollection<InviteLink> InviteLinks { get; set; } = [];
}
