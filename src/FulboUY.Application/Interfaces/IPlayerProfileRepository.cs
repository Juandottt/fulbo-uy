using FulboUY.Domain.Entities;

namespace FulboUY.Application.Interfaces;

public interface IPlayerProfileRepository
{
    Task<PlayerProfile?> GetByIdAsync(Guid id);
    Task<PlayerProfile?> GetByUserIdAsync(Guid userId);
    Task<PlayerProfile> CreateAsync(PlayerProfile profile);
    Task<PlayerProfile> UpdateAsync(PlayerProfile profile);
}
