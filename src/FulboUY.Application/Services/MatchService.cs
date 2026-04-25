using FulboUY.Application.DTOs.Match;
using FulboUY.Application.Interfaces;
using FulboUY.Domain.Entities;
using FulboUY.Domain.Enums;

namespace FulboUY.Application.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchParticipantRepository _participantRepository;
    private readonly IPlayerProfileRepository _profileRepository;

    public MatchService(
        IMatchRepository matchRepository,
        IMatchParticipantRepository participantRepository,
        IPlayerProfileRepository profileRepository)
    {
        _matchRepository = matchRepository;
        _participantRepository = participantRepository;
        _profileRepository = profileRepository;
    }

    public async Task<MatchDto> CreateAsync(string location, DateTime date, decimal fieldCost, int maxPlayers)
    {
        // Validar que la fecha sea futura
        if (date <= DateTime.UtcNow)
            throw new ArgumentException("La fecha del partido debe ser en el futuro.");

        // Validar que el precio sea positivo
        if (fieldCost <= 0)
            throw new ArgumentException("El precio de la cancha debe ser mayor a cero.");

        var match = new Match
        {
            Location = location,
            Date = date,
            FieldCost = fieldCost,
            MaxPlayers = maxPlayers,
            Status = MatchStatus.Open
        };

        var created = await _matchRepository.CreateAsync(match);
        return MapToDto(created);
    }

    public async Task<MatchDto?> GetByIdAsync(Guid id)
    {
        var match = await _matchRepository.GetByIdWithParticipantsAsync(id);
        return match == null ? null : MapToDto(match);
    }

    public async Task<IEnumerable<MatchDto>> GetAllAsync()
    {
        var matches = await _matchRepository.GetAllAsync();
        return matches.Select(MapToDto);
    }

    public async Task<MatchParticipantDto> JoinMatchAsync(Guid matchId, Guid userId)
    {
        // Obtener el partido con sus participantes
        var match = await _matchRepository.GetByIdWithParticipantsAsync(matchId)
            ?? throw new KeyNotFoundException("Partido no encontrado.");

        // El partido debe estar abierto
        if (match.Status != MatchStatus.Open)
            throw new InvalidOperationException("El partido no está disponible para inscripciones.");

        // Obtener el perfil del jugador
        var profile = await _profileRepository.GetByUserIdAsync(userId)
            ?? throw new InvalidOperationException("El usuario no tiene un perfil de jugador. Crea tu perfil primero.");

        // Verificar que no esté ya inscripto
        var alreadyJoined = await _participantRepository.ExistsAsync(matchId, profile.Id);
        if (alreadyJoined)
            throw new InvalidOperationException("Ya estás inscripto en este partido.");

        // Crear la inscripción
        var participant = new MatchParticipant
        {
            MatchId = matchId,
            PlayerProfileId = profile.Id,
            TeamNumber = 0,
            HasPaid = false
        };

        var created = await _participantRepository.CreateAsync(participant);

        // Contar participantes reales después de la inserción para evitar race conditions
        var totalParticipants = await _participantRepository.CountByMatchIdAsync(matchId);
        if (totalParticipants >= match.MaxPlayers)
        {
            match.Status = MatchStatus.Full;
            await _matchRepository.UpdateAsync(match);
        }

        return new MatchParticipantDto
        {
            Id = created.Id,
            MatchId = created.MatchId,
            UserId = profile.UserId,
            PlayerProfileId = created.PlayerProfileId,
            PlayerName = profile.Name,
            TeamNumber = created.TeamNumber,
            HasPaid = created.HasPaid,
            JoinedAt = created.JoinedAt
        };
    }

    public async Task<IEnumerable<MatchParticipantDto>> GetParticipantsAsync(Guid matchId)
    {
        // Verificar que el partido existe
        var match = await _matchRepository.GetByIdAsync(matchId)
            ?? throw new KeyNotFoundException("Partido no encontrado.");

        var participants = await _participantRepository.GetByMatchIdAsync(matchId);

        return participants.Select(p => new MatchParticipantDto
        {
            Id = p.Id,
            MatchId = p.MatchId,
            UserId = p.PlayerProfile?.UserId ?? Guid.Empty,
            PlayerProfileId = p.PlayerProfileId,
            PlayerName = p.PlayerProfile?.Name ?? "Desconocido",
            TeamNumber = p.TeamNumber,
            HasPaid = p.HasPaid,
            JoinedAt = p.JoinedAt
        });
    }

    internal static MatchDto MapToDto(Match match) => new()
    {
        Id = match.Id,
        Location = match.Location,
        Date = match.Date,
        FieldCost = match.FieldCost,
        MaxPlayers = match.MaxPlayers,
        Status = match.Status.ToString(),
        ParticipantCount = match.Participants?.Count ?? 0,
        CreatedAt = match.CreatedAt
    };
}
