using FulboUY.Application.DTOs.Match;

namespace FulboUY.Application.Interfaces;

public interface IMatchService
{
    Task<MatchDto> CreateAsync(string location, DateTime date, decimal fieldCost, int maxPlayers);
    Task<MatchDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<MatchDto>> GetAllAsync();
    Task<MatchParticipantDto> JoinMatchAsync(Guid matchId, Guid userId);
    Task<IEnumerable<MatchParticipantDto>> GetParticipantsAsync(Guid matchId);
}
