using FluentAssertions;
using FulboUY.Application.Interfaces;
using FulboUY.Application.Services;
using FulboUY.Domain.Entities;
using Moq;

namespace FulboUY.Application.Tests;

[TestClass]
public class PlayerProfileServiceTests
{
    private readonly Mock<IPlayerProfileRepository> _profileRepository = new();

    [TestMethod]
    public async Task CreateProfile_ShouldFail_WhenAlreadyExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingProfile = TestData.CreateProfile(userId: userId);
        _profileRepository.Setup(repository => repository.GetByUserIdAsync(userId)).ReturnsAsync(existingProfile);

        var service = CreateService();

        // Act
        var act = () => service.CreateAsync(userId, TestData.PlayerName, 7, 6, 8, 7);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _profileRepository.Verify(repository => repository.CreateAsync(It.IsAny<PlayerProfile>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateProfile_ShouldFail_WhenNotOwner()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var requesterId = Guid.NewGuid();
        var profile = TestData.CreateProfile(userId: ownerId);
        _profileRepository.Setup(repository => repository.GetByIdAsync(profile.Id)).ReturnsAsync(profile);

        var service = CreateService();

        // Act
        var act = () => service.UpdateAsync(profile.Id, requesterId, "New Name", null, null, null, null);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
        _profileRepository.Verify(repository => repository.UpdateAsync(It.IsAny<PlayerProfile>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateProfile_ShouldSucceed_WhenValid()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var profile = TestData.CreateProfile(userId: ownerId);
        const string updatedName = "Updated Player";
        const int updatedSpeed = 9;

        _profileRepository.Setup(repository => repository.GetByIdAsync(profile.Id)).ReturnsAsync(profile);
        _profileRepository
            .Setup(repository => repository.UpdateAsync(It.IsAny<PlayerProfile>()))
            .ReturnsAsync((PlayerProfile updatedProfile) => updatedProfile);

        var service = CreateService();

        // Act
        var result = await service.UpdateAsync(profile.Id, ownerId, updatedName, updatedSpeed, null, null, null);

        // Assert
        result.Name.Should().Be(updatedName);
        result.Speed.Should().Be(updatedSpeed);
        result.Defense.Should().Be(profile.Defense);
    }

    private PlayerProfileService CreateService()
    {
        return new PlayerProfileService(_profileRepository.Object);
    }
}
