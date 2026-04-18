using FulboUY.Domain.Entities;

namespace FulboUY.Application.Interfaces;

public interface IInviteLinkRepository
{
    Task<InviteLink?> GetByTokenAsync(Guid token);
    Task<InviteLink> CreateAsync(InviteLink link);
    Task<InviteLink> UpdateAsync(InviteLink link);
}
