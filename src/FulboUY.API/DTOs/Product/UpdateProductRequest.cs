using System.ComponentModel.DataAnnotations;

namespace FulboUY.API.DTOs.Product;

public class UpdateProductRequest
{
    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
    public decimal Price { get; set; }
}
