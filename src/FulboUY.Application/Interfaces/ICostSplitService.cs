using FulboUY.Application.DTOs.Match;

namespace FulboUY.Application.Interfaces;

public interface ICostSplitService
{
    Task<CostSplitDto> GetCostSplitAsync(Guid matchId);
    Task<MatchParticipantDto> ConfirmPaymentAsync(Guid matchId, Guid participantId);
    Task<IEnumerable<PaymentStatusDto>> GetPaymentStatusAsync(Guid matchId);
}
