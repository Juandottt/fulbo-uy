namespace FulboUY.Domain.Entities;

public class MatchParticipant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MatchId { get; set; }
    public Guid PlayerProfileId { get; set; }
    public int TeamNumber { get; set; } = 0; // 0 = sin asignar, 1 o 2 = equipo
    public bool HasPaid { get; set; } = false;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Navegación
    public Match Match { get; set; } = null!;
    public PlayerProfile PlayerProfile { get; set; } = null!;
}
