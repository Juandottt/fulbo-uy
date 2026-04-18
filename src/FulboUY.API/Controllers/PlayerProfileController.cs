using System.Security.Claims;
using FulboUY.API.DTOs.PlayerProfile;
using FulboUY.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FulboUY.API.Controllers;

[ApiController]
[Route("api/players")]
[Authorize]
public class PlayerProfileController : ControllerBase
{
    private readonly IPlayerProfileService _profileService;

    public PlayerProfileController(IPlayerProfileService profileService)
    {
        _profileService = profileService;
    }

    /// <summary>Crear perfil de jugador para el usuario autenticado</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PlayerProfileResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Create([FromBody] CreatePlayerProfileRequest request)
    {
        var userId = GetUserId();

        try
        {
            var result = await _profileService.CreateAsync(
                userId, request.Name, request.Speed, request.Defense, request.Passing, request.Shooting);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, MapToResponse(result));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>Obtener perfil por ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PlayerProfileResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _profileService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(MapToResponse(result));
    }

    /// <summary>Obtener mi perfil</summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(PlayerProfileResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetUserId();
        var result = await _profileService.GetByUserIdAsync(userId);
        return result == null ? NotFound() : Ok(MapToResponse(result));
    }

    /// <summary>Actualizar perfil de jugador</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PlayerProfileResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlayerProfileRequest request)
    {
        // Verificar que se envió al menos un campo a actualizar
        if (request.Name == null && request.Speed == null && request.Defense == null
            && request.Passing == null && request.Shooting == null)
        {
            return BadRequest(new { message = "Debe proporcionar al menos un campo para actualizar." });
        }

        var userId = GetUserId();

        try
        {
            var result = await _profileService.UpdateAsync(
                id, userId, request.Name, request.Speed, request.Defense, request.Passing, request.Shooting);

            return Ok(MapToResponse(result));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub")
            ?? throw new UnauthorizedAccessException("No se encontró el ID de usuario en el token.");
        return Guid.Parse(claim.Value);
    }

    private static PlayerProfileResponse MapToResponse(Application.DTOs.PlayerProfile.PlayerProfileDto dto) => new()
    {
        Id = dto.Id,
        UserId = dto.UserId,
        Name = dto.Name,
        Speed = dto.Speed,
        Defense = dto.Defense,
        Passing = dto.Passing,
        Shooting = dto.Shooting,
        AverageSkill = dto.AverageSkill
    };
}
