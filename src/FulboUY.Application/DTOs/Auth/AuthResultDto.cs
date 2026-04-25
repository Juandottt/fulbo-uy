namespace FulboUY.Application.DTOs.Auth;

public class AuthResultDto
{
    public Guid UserId { get; set; }
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public DateTime ExpiresAt { get; set; }
}
