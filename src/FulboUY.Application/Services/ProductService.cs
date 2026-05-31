using FulboUY.Application.DTOs.Product;
using FulboUY.Application.Interfaces;
using FulboUY.Domain.Entities;

namespace FulboUY.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> CreateAsync(string name, decimal price)
    {
        ValidateProductData(name, price);

        var product = new Product
        {
            Name = name,
            Price = price,
            IsActive = true
        };

        var created = await _productRepository.CreateAsync(product);
        return MapToDto(created);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<IEnumerable<ProductDto>> GetActiveAsync()
    {
        var products = await _productRepository.GetActiveAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, string name, decimal price)
    {
        ValidateProductData(name, price);

        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Producto no encontrado.");

        product.Name = name;
        product.Price = price;

        var updated = await _productRepository.UpdateAsync(product);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Producto no encontrado.");

        product.IsActive = false;
        await _productRepository.UpdateAsync(product);
    }

    private static void ValidateProductData(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del producto es requerido.", nameof(name));

        if (price <= 0)
            throw new ArgumentException("El precio del producto debe ser mayor a cero.", nameof(price));
    }

    private static ProductDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        IsActive = product.IsActive
    };
}
