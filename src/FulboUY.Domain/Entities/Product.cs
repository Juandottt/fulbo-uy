namespace FulboUY.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
}
