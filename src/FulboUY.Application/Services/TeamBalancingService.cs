using FulboUY.Application.DTOs.Match;
using FulboUY.Application.Interfaces;
using FulboUY.Domain.Enums;

namespace FulboUY.Application.Services;

public class TeamBalancingService : ITeamBalancingService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchParticipantRepository _participantRepository;

    public TeamBalancingService(IMatchRepository matchRepository, IMatchParticipantRepository participantRepository)
    {
        _matchRepository = matchRepository;
        _participantRepository = participantRepository;
    }

    public async Task<TeamsResultDto> BalanceTeamsAsync(Guid matchId)
    {
        var match = await _matchRepository.GetByIdWithParticipantsAsync(matchId)
            ?? throw new KeyNotFoundException("Partido no encontrado.");

        // El partido debe estar completo para balancear equipos
        if (match.Status == MatchStatus.Open)
            throw new InvalidOperationException("El partido no tiene el cupo completo todavía.");

        if (match.Status == MatchStatus.Played)
            throw new InvalidOperationException("El partido ya fue jugado.");

        if (!match.Participants.Any())
            throw new InvalidOperationException("El partido no tiene participantes.");

        // Algoritmo greedy: ordenar por promedio de habilidades (desc), asignar alternando
        var participantsWithSkill = match.Participants
            .Where(p => p.PlayerProfile != null)
            .Select(p => new
            {
                Participant = p,
                AverageSkill = (p.PlayerProfile.Speed + p.PlayerProfile.Defense +
                                p.PlayerProfile.Passing + p.PlayerProfile.Shooting) / 4.0
            })
            .OrderByDescending(x => x.AverageSkill)
            .ToList();

        // Asignar alternando: jugador 1 → equipo 1, jugador 2 → equipo 2, jugador 3 → equipo 1, etc.
        for (int i = 0; i < participantsWithSkill.Count; i++)
        {
            participantsWithSkill[i].Participant.TeamNumber = (i % 2 == 0) ? 1 : 2;
            await _participantRepository.UpdateAsync(participantsWithSkill[i].Participant);
        }

        return BuildTeamsResult(participantsWithSkill
            .Select(x => x.Participant)
            .ToList());
    }

    public async Task<TeamsResultDto> GetTeamsAsync(Guid matchId)
    {
        var match = await _matchRepository.GetByIdWithParticipantsAsync(matchId)
            ?? throw new KeyNotFoundException("Partido no encontrado.");

        return BuildTeamsResult(match.Participants.ToList());
    }

    private static TeamsResultDto BuildTeamsResult(List<Domain.Entities.MatchParticipant> participants)
    {
        var team1 = participants.Where(p => p.TeamNumber == 1).ToList();
        var team2 = participants.Where(p => p.TeamNumber == 2).ToList();

        double avgTeam1 = team1.Any() && team1.All(p => p.PlayerProfile != null)
            ? team1.Average(p => (p.PlayerProfile.Speed + p.PlayerProfile.Defense +
                                   p.PlayerProfile.Passing + p.PlayerProfile.Shooting) / 4.0)
            : 0;

        double avgTeam2 = team2.Any() && team2.All(p => p.PlayerProfile != null)
            ? team2.Average(p => (p.PlayerProfile.Speed + p.PlayerProfile.Defense +
                                   p.PlayerProfile.Passing + p.PlayerProfile.Shooting) / 4.0)
            : 0;

        return new TeamsResultDto
        {
            Team1 = new TeamDto
            {
                TeamNumber = 1,
                Players = team1.Select(MapToParticipantDto),
                AverageSkill = Math.Round(avgTeam1, 2)
            },
            Team2 = new TeamDto
            {
                TeamNumber = 2,
                Players = team2.Select(MapToParticipantDto),
                AverageSkill = Math.Round(avgTeam2, 2)
            },
            SkillDifference = Math.Round(Math.Abs(avgTeam1 - avgTeam2), 2)
        };
    }

    private static MatchParticipantDto MapToParticipantDto(Domain.Entities.MatchParticipant p) => new()
    {
        Id = p.Id,
        MatchId = p.MatchId,
        PlayerProfileId = p.PlayerProfileId,
        PlayerName = p.PlayerProfile?.Name ?? "Desconocido",
        TeamNumber = p.TeamNumber,
        HasPaid = p.HasPaid,
        JoinedAt = p.JoinedAt
    };
}
