using FluentAssertions;
using FulboUY.Application.Interfaces;
using FulboUY.Application.Services;
using FulboUY.Domain.Entities;
using FulboUY.Domain.Enums;
using Moq;

namespace FulboUY.Application.Tests;

[TestClass]
public class TeamBalancingServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepository = new();
    private readonly Mock<IMatchParticipantRepository> _participantRepository = new();

    [TestMethod]
    public async Task BalanceTeams_ShouldFail_WhenMatchNotFull()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var match = TestData.CreateMatch(id: matchId, status: MatchStatus.Open);
        _matchRepository.Setup(repository => repository.GetByIdWithParticipantsAsync(matchId)).ReturnsAsync(match);

        var service = CreateService();

        // Act
        var act = () => service.BalanceTeamsAsync(matchId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _participantRepository.Verify(repository => repository.UpdateAsync(It.IsAny<MatchParticipant>()), Times.Never);
    }

    [TestMethod]
    public async Task BalanceTeams_ShouldSucceed_WhenValid()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var match = CreateFullMatchWithFourParticipants(matchId);
        _matchRepository.Setup(repository => repository.GetByIdWithParticipantsAsync(matchId)).ReturnsAsync(match);
        _participantRepository
            .Setup(repository => repository.UpdateAsync(It.IsAny<MatchParticipant>()))
            .ReturnsAsync((MatchParticipant participant) => participant);

        var service = CreateService();

        // Act
        var result = await service.BalanceTeamsAsync(matchId);

        // Assert
        result.Team1.Players.Should().HaveCount(2);
        result.Team2.Players.Should().HaveCount(2);
        _participantRepository.Verify(repository => repository.UpdateAsync(It.IsAny<MatchParticipant>()), Times.Exactly(4));
    }

    [TestMethod]
    public async Task BalanceTeams_ShouldReturnBalancedTeams()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var match = CreateFullMatchWithFourParticipants(matchId);
        _matchRepository.Setup(repository => repository.GetByIdWithParticipantsAsync(matchId)).ReturnsAsync(match);
        _participantRepository
            .Setup(repository => repository.UpdateAsync(It.IsAny<MatchParticipant>()))
            .ReturnsAsync((MatchParticipant participant) => participant);

        var service = CreateService();

        // Act
        var result = await service.BalanceTeamsAsync(matchId);

        // Assert
        result.Team1.AverageSkill.Should().Be(7);
        result.Team2.AverageSkill.Should().Be(6);
        result.SkillDifference.Should().Be(1);
    }

    private TeamBalancingService CreateService()
    {
        return new TeamBalancingService(_matchRepository.Object, _participantRepository.Object);
    }

    private static FulboUY.Domain.Entities.Match CreateFullMatchWithFourParticipants(Guid matchId)
    {
        var participants = new[]
        {
            CreateParticipantWithAverageSkill(matchId, 10),
            CreateParticipantWithAverageSkill(matchId, 9),
            CreateParticipantWithAverageSkill(matchId, 4),
            CreateParticipantWithAverageSkill(matchId, 3)
        };

        return TestData.CreateMatch(
            id: matchId,
            status: MatchStatus.Full,
            maxPlayers: 4,
            participants: participants);
    }

    private static MatchParticipant CreateParticipantWithAverageSkill(Guid matchId, int skill)
    {
        var profile = TestData.CreateProfile(speed: skill, defense: skill, passing: skill, shooting: skill);
        return TestData.CreateParticipant(matchId: matchId, profile: profile);
    }
}
