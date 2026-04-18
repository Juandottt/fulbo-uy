using FulboUY.Domain.Entities;

namespace FulboUY.Application.Interfaces;

public interface IMatchParticipantRepository
{
    Task<MatchParticipant?> GetByIdAsync(Guid id);
    Task<IEnumerable<MatchParticipant>> GetByMatchIdAsync(Guid matchId);
    Task<bool> ExistsAsync(Guid matchId, Guid playerProfileId);
    Task<MatchParticipant> CreateAsync(MatchParticipant participant);
    Task<MatchParticipant> UpdateAsync(MatchParticipant participant);
    Task<int> CountByMatchIdAsync(Guid matchId);
}
