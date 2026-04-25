using FluentAssertions;
using FulboUY.Application.Interfaces;
using FulboUY.Application.Services;
using FulboUY.Domain.Entities;
using Moq;

namespace FulboUY.Application.Tests;

[TestClass]
public class CostSplitServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepository = new();
    private readonly Mock<IMatchParticipantRepository> _participantRepository = new();

    [TestMethod]
    public async Task SplitCost_ShouldFail_WhenNoParticipants()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var match = TestData.CreateMatch(id: matchId, participants: []);
        _matchRepository.Setup(repository => repository.GetByIdWithParticipantsAsync(matchId)).ReturnsAsync(match);

        var service = CreateService();

        // Act
        var act = () => service.GetCostSplitAsync(matchId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [TestMethod]
    public async Task SplitCost_ShouldCalculateCorrectly()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var participants = new[]
        {
            TestData.CreateParticipant(matchId: matchId, hasPaid: true),
            TestData.CreateParticipant(matchId: matchId, hasPaid: false),
            TestData.CreateParticipant(matchId: matchId, hasPaid: false)
        };
        var match = TestData.CreateMatch(id: matchId, fieldCost: 3000m, participants: participants);
        _matchRepository.Setup(repository => repository.GetByIdWithParticipantsAsync(matchId)).ReturnsAsync(match);

        var service = CreateService();

        // Act
        var result = await service.GetCostSplitAsync(matchId);

        // Assert
        result.TotalCost.Should().Be(3000m);
        result.PlayerCount.Should().Be(3);
        result.CostPerPlayer.Should().Be(1000m);
        result.PaidCount.Should().Be(1);
        result.PendingCount.Should().Be(2);
    }

    [TestMethod]
    public async Task ConfirmPayment_ShouldFail_WhenAlreadyPaid()
    {
        // Arrange
        var matchId = Guid.NewGuid();
        var participant = TestData.CreateParticipant(matchId: matchId, hasPaid: true);
        var match = TestData.CreateMatch(id: matchId);

        _matchRepository.Setup(repository => repository.GetByIdAsync(matchId)).ReturnsAsync(match);
        _participantRepository.Setup(repository => repository.GetByIdAsync(participant.Id)).ReturnsAsync(participant);

        var service = CreateService();

        // Act
        var act = () => service.ConfirmPaymentAsync(matchId, participant.Id);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _participantRepository.Verify(repository => repository.UpdateAsync(It.IsAny<MatchParticipant>()), Times.Never);
    }

    private CostSplitService CreateService()
    {
        return new CostSplitService(_matchRepository.Object, _participantRepository.Object);
    }
}
