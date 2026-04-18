using FulboUY.Application.DTOs.Match;

namespace FulboUY.Application.Interfaces;

public interface ITeamBalancingService
{
    Task<TeamsResultDto> BalanceTeamsAsync(Guid matchId);
    Task<TeamsResultDto> GetTeamsAsync(Guid matchId);
}
