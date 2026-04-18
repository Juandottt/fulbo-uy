using FulboUY.Domain.Entities;

namespace FulboUY.Application.Interfaces;

public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id);
    Task<Match?> GetByIdWithParticipantsAsync(Guid id);
    Task<IEnumerable<Match>> GetAllAsync();
    Task<Match> CreateAsync(Match match);
    Task<Match> UpdateAsync(Match match);
}
