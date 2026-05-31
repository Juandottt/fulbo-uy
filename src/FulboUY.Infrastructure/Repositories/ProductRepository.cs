using FulboUY.Application.Interfaces;
using FulboUY.Domain.Entities;
using FulboUY.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FulboUY.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id) =>
        await _context.Products.FindAsync(id);

    public async Task<IEnumerable<Product>> GetActiveAsync() =>
        await _context.Products
            .Where(product => product.IsActive)
            .OrderBy(product => product.Name)
            .ToListAsync();

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        var entry = _context.Entry(product);
        if (entry.State == EntityState.Detached)
            _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }
}
