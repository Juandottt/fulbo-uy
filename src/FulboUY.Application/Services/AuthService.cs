using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FulboUY.Application.DTOs.Auth;
using FulboUY.Application.Interfaces;
using FulboUY.Application.Settings;
using FulboUY.Domain.Entities;
using FulboUY.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FulboUY.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResultDto> RegisterAsync(string email, string password)
    {
        // Verificar si el email ya existe
        if (await _userRepository.ExistsAsync(email))
            throw new InvalidOperationException("El email ya está registrado.");

        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = UserRole.Player
        };

        await _userRepository.CreateAsync(user);
        return GenerateToken(user);
    }

    public async Task<AuthResultDto> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
            ?? throw new UnauthorizedAccessException("Credenciales inválidas.");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        return GenerateToken(user);
    }

    private AuthResultDto GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new AuthResultDto
        {
            UserId = user.Id,
            Token = TokenHandler.WriteToken(token),
            Email = user.Email,
            Role = user.Role.ToString(),
            ExpiresAt = expiration
        };
    }
}
