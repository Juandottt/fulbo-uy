using FluentAssertions;
using FulboUY.Application.Interfaces;
using FulboUY.Application.Services;
using FulboUY.Domain.Entities;
using FulboUY.Domain.Enums;
using Moq;

namespace FulboUY.Application.Tests;

[TestClass]
public class MatchServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepository = new();
    private readonly Mock<IMatchParticipantRepository> _participantRepository = new();
    private readonly Mock<IPlayerProfileRepository> _profileRepository = new();

    [TestMethod]
    public async Task CreateMatch_ShouldFail_WhenInvalidData()
    {
        // Arrange
        var service = CreateService();
        var pastDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var act = () => service.CreateAsync(TestData.MatchLocation, pastDate, TestData.FieldCost, TestData.MaxPlayers);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
        _matchRepository.Verify(repository => repository.CreateAsync(It.IsAny<FulboUY.Domain.Entities.Match>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateMatch_ShouldSucceed_WhenValid()
    {
        // Arrange
        _matchRepository
            .Setup(repository => repository.CreateAsync(It.IsAny<FulboUY.Domain.Entities.Match>()))
            .ReturnsAsync((FulboUY.Domain.Entities.Match match) => match);

        var service = CreateService();
        var futureDate = DateTime.UtcNow.AddDays(3);

        // Act
        var result = await service.CreateAsync(TestData.MatchLocation, futureDate, TestData.FieldCost, TestData.MaxPlayers);

        // Assert
        result.Location.Should().Be(TestData.MatchLocation);
        result.Date.Should().Be(futureDate);
        result.FieldCost.Should().Be(TestData.FieldCost);
        result.MaxPlayers.Should().Be(TestData.MaxPlayers);
        result.Status.Should().Be(MatchStatus.Open.ToString());
    }

    [TestMethod]
    public async Task JoinMatch_ShouldFail_WhenAlreadyJoined()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var profile = TestData.CreateProfile(userId: userId);
        var match = TestData.CreateMatch(id: matchId);

        _matchRepository.Setup(repository => repository.GetByIdWithParticipantsAsync(matchId)).ReturnsAsync(match);
        _profileRepository.Setup(repository => repository.GetByUserIdAsync(userId)).ReturnsAsync(profile);
        _participantRepository.Setup(repository => repository.ExistsAsync(matchId, profile.Id)).ReturnsAsync(true);

        var service = CreateService();

        // Act
        var act = () => service.JoinMatchAsync(matchId, userId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _participantRepository.Verify(repository => repository.CreateAsync(It.IsAny<MatchParticipant>()), Times.Never);
    }

    [TestMethod]
    public async Task JoinMatch_ShouldFail_WhenMatchIsFull()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var match = TestData.CreateMatch(id: matchId, status: MatchStatus.Full);

        _matchRepository.Setup(repository => repository.GetByIdWithParticipantsAsync(matchId)).ReturnsAsync(match);

        var service = CreateService();

        // Act
        var act = () => service.JoinMatchAsync(matchId, userId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _profileRepository.Verify(repository => repository.GetByUserIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [TestMethod]
    public async Task JoinMatch_ShouldSucceed_WhenValid()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var profile = TestData.CreateProfile(userId: userId);
        var match = TestData.CreateMatch(id: matchId);

        _matchRepository.Setup(repository => repository.GetByIdWithParticipantsAsync(matchId)).ReturnsAsync(match);
        _profileRepository.Setup(repository => repository.GetByUserIdAsync(userId)).ReturnsAsync(profile);
        _participantRepository.Setup(repository => repository.ExistsAsync(matchId, profile.Id)).ReturnsAsync(false);
        _participantRepository
            .Setup(repository => repository.CreateAsync(It.IsAny<MatchParticipant>()))
            .ReturnsAsync((MatchParticipant participant) => participant);
        _participantRepository.Setup(repository => repository.CountByMatchIdAsync(matchId)).ReturnsAsync(1);

        var service = CreateService();

        // Act
        var result = await service.JoinMatchAsync(matchId, userId);

        // Assert
        result.MatchId.Should().Be(matchId);
        result.UserId.Should().Be(userId);
        result.PlayerProfileId.Should().Be(profile.Id);
        result.PlayerName.Should().Be(profile.Name);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnCorrectParticipantCount()
    {
        // Arrange
        var match = TestData.CreateMatch(participants:
        [
            TestData.CreateParticipant(),
            TestData.CreateParticipant(),
            TestData.CreateParticipant()
        ]);

        _matchRepository.Setup(repository => repository.GetAllAsync()).ReturnsAsync([match]);

        var service = CreateService();

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Single().ParticipantCount.Should().Be(3);
    }

    private MatchService CreateService()
    {
        return new MatchService(
            _matchRepository.Object,
            _participantRepository.Object,
            _profileRepository.Object);
    }
}
