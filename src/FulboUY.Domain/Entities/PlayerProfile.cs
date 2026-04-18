namespace FulboUY.Domain.Entities;

public class PlayerProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public required string Name { get; set; }

    // Atributos futbolísticos (1-10)
    public int Speed { get; set; }
    public int Defense { get; set; }
    public int Passing { get; set; }
    public int Shooting { get; set; }

    // Navegación
    public User User { get; set; } = null!;
    public ICollection<MatchParticipant> Participations { get; set; } = [];
}
