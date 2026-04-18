using FulboUY.API.DTOs.Auth;
using FulboUY.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FulboUY.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Registrar nuevo usuario</summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), 201)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request.Email, request.Password);
            var response = new AuthResponse
            {
                Token = result.Token,
                Email = result.Email,
                Role = result.Role,
                ExpiresAt = result.ExpiresAt
            };
            return StatusCode(201, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>Iniciar sesión</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);
            var response = new AuthResponse
            {
                Token = result.Token,
                Email = result.Email,
                Role = result.Role,
                ExpiresAt = result.ExpiresAt
            };
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
