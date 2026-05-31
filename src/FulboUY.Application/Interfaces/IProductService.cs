using FulboUY.Application.DTOs.Product;

namespace FulboUY.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> CreateAsync(string name, decimal price);
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetActiveAsync();
    Task<ProductDto> UpdateAsync(Guid id, string name, decimal price);
    Task DeleteAsync(Guid id);
}
