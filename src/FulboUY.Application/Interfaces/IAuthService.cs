using FulboUY.Application.DTOs.Auth;

namespace FulboUY.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> RegisterAsync(string email, string password);
    Task<AuthResultDto> LoginAsync(string email, string password);
}
