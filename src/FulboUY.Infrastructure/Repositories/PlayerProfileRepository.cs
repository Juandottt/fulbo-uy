using FulboUY.Application.Interfaces;
using FulboUY.Domain.Entities;
using FulboUY.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FulboUY.Infrastructure.Repositories;

public class PlayerProfileRepository : IPlayerProfileRepository
{
    private readonly AppDbContext _context;

    public PlayerProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PlayerProfile?> GetByIdAsync(Guid id) =>
        await _context.PlayerProfiles.FindAsync(id);

    public async Task<PlayerProfile?> GetByUserIdAsync(Guid userId) =>
        await _context.PlayerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task<PlayerProfile> CreateAsync(PlayerProfile profile)
    {
        _context.PlayerProfiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<PlayerProfile> UpdateAsync(PlayerProfile profile)
    {
        var entry = _context.Entry(profile);
        if (entry.State == EntityState.Detached)
            _context.PlayerProfiles.Update(profile);
        await _context.SaveChangesAsync();
        return profile;
    }
}
