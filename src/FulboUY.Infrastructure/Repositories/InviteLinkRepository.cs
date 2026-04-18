using FulboUY.Application.Interfaces;
using FulboUY.Domain.Entities;
using FulboUY.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FulboUY.Infrastructure.Repositories;

public class InviteLinkRepository : IInviteLinkRepository
{
    private readonly AppDbContext _context;

    public InviteLinkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InviteLink?> GetByTokenAsync(Guid token) =>
        await _context.InviteLinks.FirstOrDefaultAsync(il => il.Token == token);

    public async Task<InviteLink> CreateAsync(InviteLink link)
    {
        _context.InviteLinks.Add(link);
        await _context.SaveChangesAsync();
        return link;
    }

    public async Task<InviteLink> UpdateAsync(InviteLink link)
    {
        var entry = _context.Entry(link);
        if (entry.State == EntityState.Detached)
            _context.InviteLinks.Update(link);
        await _context.SaveChangesAsync();
        return link;
    }
}
