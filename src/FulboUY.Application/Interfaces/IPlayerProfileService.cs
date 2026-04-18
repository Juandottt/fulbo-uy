using FulboUY.Application.DTOs.PlayerProfile;

namespace FulboUY.Application.Interfaces;

public interface IPlayerProfileService
{
    Task<PlayerProfileDto> CreateAsync(Guid userId, string name, int speed, int defense, int passing, int shooting);
    Task<PlayerProfileDto?> GetByIdAsync(Guid id);
    Task<PlayerProfileDto?> GetByUserIdAsync(Guid userId);
    Task<PlayerProfileDto> UpdateAsync(Guid id, Guid userId, string? name, int? speed, int? defense, int? passing, int? shooting);
}
