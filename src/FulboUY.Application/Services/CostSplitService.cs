using FulboUY.Application.DTOs.Match;
using FulboUY.Application.Interfaces;

namespace FulboUY.Application.Services;

public class CostSplitService : ICostSplitService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchParticipantRepository _participantRepository;

    public CostSplitService(IMatchRepository matchRepository, IMatchParticipantRepository participantRepository)
    {
        _matchRepository = matchRepository;
        _participantRepository = participantRepository;
    }

    public async Task<CostSplitDto> GetCostSplitAsync(Guid matchId)
    {
        var match = await _matchRepository.GetByIdWithParticipantsAsync(matchId)
            ?? throw new KeyNotFoundException("Partido no encontrado.");

        var participants = match.Participants.ToList();
        var playerCount = participants.Count;

        if (playerCount == 0)
            throw new InvalidOperationException("El partido no tiene participantes.");

        var costPerPlayer = Math.Round(match.FieldCost / playerCount, 2);
        var paidCount = participants.Count(p => p.HasPaid);

        return new CostSplitDto
        {
            MatchId = matchId,
            TotalCost = match.FieldCost,
            PlayerCount = playerCount,
            CostPerPlayer = costPerPlayer,
            PaidCount = paidCount,
            PendingCount = playerCount - paidCount
        };
    }

    public async Task<MatchParticipantDto> ConfirmPaymentAsync(Guid matchId, Guid participantId)
    {
        // Verificar que el partido existe
        var match = await _matchRepository.GetByIdAsync(matchId)
            ?? throw new KeyNotFoundException("Partido no encontrado.");

        // Verificar que el participante existe y pertenece a este partido
        var participant = await _participantRepository.GetByIdAsync(participantId)
            ?? throw new KeyNotFoundException("Participante no encontrado.");

        if (participant.MatchId != matchId)
            throw new ArgumentException("El participante no pertenece a este partido.");

        if (participant.HasPaid)
            throw new InvalidOperationException("El jugador ya confirmó su pago.");

        participant.HasPaid = true;
        var updated = await _participantRepository.UpdateAsync(participant);

        return new MatchParticipantDto
        {
            Id = updated.Id,
            MatchId = updated.MatchId,
            PlayerProfileId = updated.PlayerProfileId,
            PlayerName = updated.PlayerProfile?.Name ?? "Desconocido",
            TeamNumber = updated.TeamNumber,
            HasPaid = updated.HasPaid,
            JoinedAt = updated.JoinedAt
        };
    }

    public async Task<IEnumerable<PaymentStatusDto>> GetPaymentStatusAsync(Guid matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId)
            ?? throw new KeyNotFoundException("Partido no encontrado.");

        var participants = await _participantRepository.GetByMatchIdAsync(matchId);

        return participants.Select(p => new PaymentStatusDto
        {
            ParticipantId = p.Id,
            PlayerProfileId = p.PlayerProfileId,
            PlayerName = p.PlayerProfile?.Name ?? "Desconocido",
            HasPaid = p.HasPaid
        });
    }
}
