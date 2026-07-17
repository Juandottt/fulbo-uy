using FulboUY.Domain.Entities;

namespace FulboUY.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetActiveAsync();
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
}
