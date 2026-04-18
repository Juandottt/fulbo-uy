using System.ComponentModel.DataAnnotations;

namespace FulboUY.API.DTOs.Match;

public class CreateMatchRequest
{
    [Required]
    [MaxLength(200)]
    public required string Location { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
    public decimal FieldCost { get; set; }

    [Range(10, 22, ErrorMessage = "El máximo de jugadores debe estar entre 10 y 22.")]
    public int MaxPlayers { get; set; }
}
