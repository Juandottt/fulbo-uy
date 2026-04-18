using FulboUY.Application.DTOs.PlayerProfile;
using FulboUY.Application.Interfaces;
using FulboUY.Domain.Entities;

namespace FulboUY.Application.Services;

public class PlayerProfileService : IPlayerProfileService
{
    private readonly IPlayerProfileRepository _profileRepository;

    public PlayerProfileService(IPlayerProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<PlayerProfileDto> CreateAsync(Guid userId, string name, int speed, int defense, int passing, int shooting)
    {
        // Verificar que el usuario no tenga ya un perfil
        var existing = await _profileRepository.GetByUserIdAsync(userId);
        if (existing != null)
            throw new InvalidOperationException("El usuario ya tiene un perfil de jugador.");

        var profile = new PlayerProfile
        {
            UserId = userId,
            Name = name,
            Speed = speed,
            Defense = defense,
            Passing = passing,
            Shooting = shooting
        };

        var created = await _profileRepository.CreateAsync(profile);
        return MapToDto(created);
    }

    public async Task<PlayerProfileDto?> GetByIdAsync(Guid id)
    {
        var profile = await _profileRepository.GetByIdAsync(id);
        return profile == null ? null : MapToDto(profile);
    }

    public async Task<PlayerProfileDto?> GetByUserIdAsync(Guid userId)
    {
        var profile = await _profileRepository.GetByUserIdAsync(userId);
        return profile == null ? null : MapToDto(profile);
    }

    public async Task<PlayerProfileDto> UpdateAsync(Guid id, Guid userId, string? name, int? speed, int? defense, int? passing, int? shooting)
    {
        var profile = await _profileRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Perfil no encontrado.");

        // Solo el dueño puede actualizar su perfil
        if (profile.UserId != userId)
            throw new UnauthorizedAccessException("No tenés permiso para modificar este perfil.");

        if (name != null) profile.Name = name;
        if (speed.HasValue) profile.Speed = speed.Value;
        if (defense.HasValue) profile.Defense = defense.Value;
        if (passing.HasValue) profile.Passing = passing.Value;
        if (shooting.HasValue) profile.Shooting = shooting.Value;

        var updated = await _profileRepository.UpdateAsync(profile);
        return MapToDto(updated);
    }

    private static PlayerProfileDto MapToDto(PlayerProfile profile) => new()
    {
        Id = profile.Id,
        UserId = profile.UserId,
        Name = profile.Name,
        Speed = profile.Speed,
        Defense = profile.Defense,
        Passing = profile.Passing,
        Shooting = profile.Shooting
    };
}
