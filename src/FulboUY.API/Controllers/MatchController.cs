using FulboUY.API.DTOs.Match;
using FulboUY.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FulboUY.API.Controllers;

[ApiController]
[Route("api/matches")]
[Authorize]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly ITeamBalancingService _teamBalancingService;
    private readonly ICostSplitService _costSplitService;

    public MatchController(
        IMatchService matchService,
        ITeamBalancingService teamBalancingService,
        ICostSplitService costSplitService)
    {
        _matchService = matchService;
        _teamBalancingService = teamBalancingService;
        _costSplitService = costSplitService;
    }

    /// <summary>Crear un partido (solo Admin)</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(MatchResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> Create([FromBody] CreateMatchRequest request)
    {
        try
        {
            var result = await _matchService.CreateAsync(
                request.Location, request.Date, request.FieldCost, request.MaxPlayers);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, MapToResponse(result));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Listar todos los partidos</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MatchResponse>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var results = await _matchService.GetAllAsync();
        return Ok(results.Select(MapToResponse));
    }

    /// <summary>Obtener partido por ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MatchResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _matchService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(MapToResponse(result));
    }

    /// <summary>Inscribirse a un partido</summary>
    [HttpPost("{id:guid}/join")]
    [ProducesResponseType(typeof(MatchParticipantResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Join(Guid id)
    {
        var userId = GetUserId();

        try
        {
            var result = await _matchService.JoinMatchAsync(id, userId);
            return StatusCode(201, MapToParticipantResponse(result));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            // "Ya estás inscripto" → 409, "partido no disponible" → 400
            if (ex.Message.Contains("inscripto"))
                return Conflict(new { message = ex.Message });
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Listar participantes de un partido</summary>
    [HttpGet("{id:guid}/participants")]
    [ProducesResponseType(typeof(IEnumerable<MatchParticipantResponse>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetParticipants(Guid id)
    {
        try
        {
            var results = await _matchService.GetParticipantsAsync(id);
            return Ok(results.Select(MapToParticipantResponse));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>Balancear equipos (solo Admin, partido completo)</summary>
    [HttpPost("{id:guid}/balance-teams")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(TeamsResultResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> BalanceTeams(Guid id)
    {
        try
        {
            var result = await _teamBalancingService.BalanceTeamsAsync(id);
            return Ok(MapToTeamsResponse(result));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Ver equipos asignados</summary>
    [HttpGet("{id:guid}/teams")]
    [ProducesResponseType(typeof(TeamsResultResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetTeams(Guid id)
    {
        try
        {
            var result = await _teamBalancingService.GetTeamsAsync(id);
            return Ok(MapToTeamsResponse(result));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>Ver división de costos del partido</summary>
    [HttpGet("{id:guid}/cost-split")]
    [ProducesResponseType(typeof(CostSplitResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCostSplit(Guid id)
    {
        try
        {
            var result = await _costSplitService.GetCostSplitAsync(id);
            return Ok(new CostSplitResponse
            {
                MatchId = result.MatchId,
                TotalCost = result.TotalCost,
                PlayerCount = result.PlayerCount,
                CostPerPlayer = result.CostPerPlayer,
                PaidCount = result.PaidCount,
                PendingCount = result.PendingCount
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Confirmar pago de un participante (solo Admin)</summary>
    [HttpPost("{id:guid}/participants/{participantId:guid}/pay")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(MatchParticipantResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> ConfirmPayment(Guid id, Guid participantId)
    {
        try
        {
            var result = await _costSplitService.ConfirmPaymentAsync(id, participantId);
            return Ok(MapToParticipantResponse(result));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Ver estado de pagos del partido</summary>
    [HttpGet("{id:guid}/payment-status")]
    [ProducesResponseType(typeof(IEnumerable<PaymentStatusResponse>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPaymentStatus(Guid id)
    {
        try
        {
            var results = await _costSplitService.GetPaymentStatusAsync(id);
            return Ok(results.Select(r => new PaymentStatusResponse
            {
                ParticipantId = r.ParticipantId,
                PlayerName = r.PlayerName,
                HasPaid = r.HasPaid
            }));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub")
            ?? throw new UnauthorizedAccessException("No se encontró el ID de usuario en el token.");
        return Guid.Parse(claim.Value);
    }

    private static MatchParticipantResponse MapToParticipantResponse(Application.DTOs.Match.MatchParticipantDto dto) => new()
    {
        Id = dto.Id,
        UserId = dto.UserId,
        PlayerProfileId = dto.PlayerProfileId,
        PlayerName = dto.PlayerName,
        TeamNumber = dto.TeamNumber,
        HasPaid = dto.HasPaid,
        JoinedAt = dto.JoinedAt
    };

    private static TeamsResultResponse MapToTeamsResponse(Application.DTOs.Match.TeamsResultDto dto) => new()
    {
        Team1 = new TeamResponse
        {
            TeamNumber = dto.Team1.TeamNumber,
            Players = dto.Team1.Players.Select(MapToParticipantResponse),
            AverageSkill = dto.Team1.AverageSkill
        },
        Team2 = new TeamResponse
        {
            TeamNumber = dto.Team2.TeamNumber,
            Players = dto.Team2.Players.Select(MapToParticipantResponse),
            AverageSkill = dto.Team2.AverageSkill
        },
        SkillDifference = dto.SkillDifference
    };

    private static MatchResponse MapToResponse(Application.DTOs.Match.MatchDto dto) => new()
    {
        Id = dto.Id,
        Location = dto.Location,
        Date = dto.Date,
        FieldCost = dto.FieldCost,
        MaxPlayers = dto.MaxPlayers,
        Status = dto.Status,
        ParticipantCount = dto.ParticipantCount,
        CreatedAt = dto.CreatedAt
    };
}
