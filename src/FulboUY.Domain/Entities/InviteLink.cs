namespace FulboUY.Domain.Entities;

public class InviteLink
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MatchId { get; set; }
    public Guid Token { get; set; } = Guid.NewGuid();
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navegación
    public Match Match { get; set; } = null!;
}
