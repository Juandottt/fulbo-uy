using FluentAssertions;
using FulboUY.Application.Interfaces;
using FulboUY.Application.Services;
using FulboUY.Application.Settings;
using FulboUY.Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;

namespace FulboUY.Application.Tests;

[TestClass]
public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();

    [TestMethod]
    public async Task Register_ShouldFail_WhenEmailAlreadyExists()
    {
        // Arrange
        _userRepository
            .Setup(repository => repository.ExistsAsync(TestData.ValidEmail))
            .ReturnsAsync(true);

        var service = CreateService();

        // Act
        var act = () => service.RegisterAsync(TestData.ValidEmail, TestData.ValidPassword);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _userRepository.Verify(repository => repository.CreateAsync(It.IsAny<User>()), Times.Never);
    }

    [TestMethod]
    public async Task Register_ShouldCreateUser_WhenValid()
    {
        // Arrange
        User? createdUser = null;
        _userRepository
            .Setup(repository => repository.ExistsAsync(TestData.ValidEmail))
            .ReturnsAsync(false);
        _userRepository
            .Setup(repository => repository.CreateAsync(It.IsAny<User>()))
            .Callback<User>(user => createdUser = user)
            .ReturnsAsync((User user) => user);

        var service = CreateService();

        // Act
        var result = await service.RegisterAsync(TestData.ValidEmail, TestData.ValidPassword);

        // Assert
        createdUser.Should().NotBeNull();
        createdUser!.Email.Should().Be(TestData.ValidEmail);
        createdUser.PasswordHash.Should().NotBe(TestData.ValidPassword);
        BCrypt.Net.BCrypt.Verify(TestData.ValidPassword, createdUser.PasswordHash).Should().BeTrue();
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.Email.Should().Be(TestData.ValidEmail);
    }

    [TestMethod]
    public async Task Login_ShouldFail_WithInvalidCredentials()
    {
        // Arrange
        var user = TestData.CreateUser();
        _userRepository
            .Setup(repository => repository.GetByEmailAsync(TestData.ValidEmail))
            .ReturnsAsync(user);

        var service = CreateService();

        // Act
        var act = () => service.LoginAsync(TestData.ValidEmail, "WrongPassword123!");

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [TestMethod]
    public async Task Login_ShouldReturnJwt_WhenValid()
    {
        // Arrange
        var user = TestData.CreateUser();
        _userRepository
            .Setup(repository => repository.GetByEmailAsync(TestData.ValidEmail))
            .ReturnsAsync(user);

        var service = CreateService();

        // Act
        var result = await service.LoginAsync(TestData.ValidEmail, TestData.ValidPassword);

        // Assert
        result.UserId.Should().Be(user.Id);
        result.Email.Should().Be(user.Email);
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    private AuthService CreateService()
    {
        var settings = Options.Create(new JwtSettings
        {
            Secret = "test-secret-key-with-more-than-32-chars",
            Issuer = "FulboUY.Tests",
            Audience = "FulboUY.Tests",
            ExpirationHours = 2
        });

        return new AuthService(_userRepository.Object, settings);
    }
}
