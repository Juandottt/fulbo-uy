namespace FulboUY.API.DTOs.Auth;

public class AuthResponse
{
    public Guid UserId { get; set; }
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public DateTime ExpiresAt { get; set; }
}
