using FulboUY.Domain.Entities;
using FulboUY.Domain.Enums;

namespace FulboUY.Application.Tests;

internal static class TestData
{
    public const string ValidEmail = "player@fulbouy.test";
    public const string ValidPassword = "StrongPassword123!";
    public const string PlayerName = "Test Player";
    public const string MatchLocation = "Central Field";
    public const decimal FieldCost = 2400m;
    public const int MaxPlayers = 10;

    public static User CreateUser(
        Guid? id = null,
        string email = ValidEmail,
        string? passwordHash = null,
        UserRole role = UserRole.Player)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash ?? BCrypt.Net.BCrypt.HashPassword(ValidPassword),
            Role = role
        };
    }

    public static PlayerProfile CreateProfile(
        Guid? id = null,
        Guid? userId = null,
        string name = PlayerName,
        int speed = 7,
        int defense = 6,
        int passing = 8,
        int shooting = 7)
    {
        return new PlayerProfile
        {
            Id = id ?? Guid.NewGuid(),
            UserId = userId ?? Guid.NewGuid(),
            Name = name,
            Speed = speed,
            Defense = defense,
            Passing = passing,
            Shooting = shooting
        };
    }

    public static Match CreateMatch(
        Guid? id = null,
        MatchStatus status = MatchStatus.Open,
        int maxPlayers = MaxPlayers,
        decimal fieldCost = FieldCost,
        DateTime? date = null,
        IEnumerable<MatchParticipant>? participants = null)
    {
        return new Match
        {
            Id = id ?? Guid.NewGuid(),
            Location = MatchLocation,
            Date = date ?? DateTime.UtcNow.AddDays(7),
            FieldCost = fieldCost,
            MaxPlayers = maxPlayers,
            Status = status,
            Participants = participants?.ToList() ?? []
        };
    }

    public static MatchParticipant CreateParticipant(
        Guid? id = null,
        Guid? matchId = null,
        PlayerProfile? profile = null,
        int teamNumber = 0,
        bool hasPaid = false)
    {
        var playerProfile = profile ?? CreateProfile();

        return new MatchParticipant
        {
            Id = id ?? Guid.NewGuid(),
            MatchId = matchId ?? Guid.NewGuid(),
            PlayerProfileId = playerProfile.Id,
            PlayerProfile = playerProfile,
            TeamNumber = teamNumber,
            HasPaid = hasPaid,
            JoinedAt = DateTime.UtcNow
        };
    }
}
