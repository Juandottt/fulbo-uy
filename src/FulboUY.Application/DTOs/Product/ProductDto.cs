namespace FulboUY.Application.DTOs.Product;

public class ProductDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
