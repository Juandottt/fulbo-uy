using FulboUY.Application.Interfaces;
using FulboUY.Domain.Entities;
using FulboUY.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FulboUY.Infrastructure.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly AppDbContext _context;

    public MatchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Match?> GetByIdAsync(Guid id) =>
        await _context.Matches.FindAsync(id);

    public async Task<Match?> GetByIdWithParticipantsAsync(Guid id) =>
        await _context.Matches
            .Include(m => m.Participants)
                .ThenInclude(p => p.PlayerProfile)
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task<IEnumerable<Match>> GetAllAsync() =>
        await _context.Matches.OrderByDescending(m => m.Date).ToListAsync();

    public async Task<Match> CreateAsync(Match match)
    {
        _context.Matches.Add(match);
        await _context.SaveChangesAsync();
        return match;
    }

    public async Task<Match> UpdateAsync(Match match)
    {
        var entry = _context.Entry(match);
        if (entry.State == EntityState.Detached)
            _context.Matches.Update(match);
        await _context.SaveChangesAsync();
        return match;
    }
}
