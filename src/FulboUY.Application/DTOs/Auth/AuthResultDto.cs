namespace FulboUY.Application.DTOs.Auth;

public class AuthResultDto
{
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public DateTime ExpiresAt { get; set; }
}
