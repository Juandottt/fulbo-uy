using FulboUY.Application.Interfaces;
using FulboUY.Domain.Entities;
using FulboUY.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FulboUY.Infrastructure.Repositories;

public class MatchParticipantRepository : IMatchParticipantRepository
{
    private readonly AppDbContext _context;

    public MatchParticipantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MatchParticipant?> GetByIdAsync(Guid id) =>
        await _context.MatchParticipants.FindAsync(id);

    public async Task<IEnumerable<MatchParticipant>> GetByMatchIdAsync(Guid matchId) =>
        await _context.MatchParticipants
            .Include(mp => mp.PlayerProfile)
            .Where(mp => mp.MatchId == matchId)
            .ToListAsync();

    public async Task<bool> ExistsAsync(Guid matchId, Guid playerProfileId) =>
        await _context.MatchParticipants
            .AnyAsync(mp => mp.MatchId == matchId && mp.PlayerProfileId == playerProfileId);

    public async Task<MatchParticipant> CreateAsync(MatchParticipant participant)
    {
        _context.MatchParticipants.Add(participant);
        await _context.SaveChangesAsync();
        return participant;
    }

    public async Task<MatchParticipant> UpdateAsync(MatchParticipant participant)
    {
        var entry = _context.Entry(participant);
        if (entry.State == EntityState.Detached)
            _context.MatchParticipants.Update(participant);
        await _context.SaveChangesAsync();
        return participant;
    }

    public async Task<int> CountByMatchIdAsync(Guid matchId) =>
        await _context.MatchParticipants.CountAsync(mp => mp.MatchId == matchId);
}
