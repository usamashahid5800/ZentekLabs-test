using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Domain;

namespace Test.Services;

public class EfProductService : IProductService
{
    private readonly ProductsDbContext _dbContext;

    public EfProductService(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> CreateAsync(string name, string colour, decimal price, CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Colour = colour.Trim(),
            Price = price,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return product;
    }

    public async Task<IReadOnlyCollection<Product>> GetAllAsync(string? colour = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(colour))
        {
            var normalizedColour = colour.Trim();
            query = query.Where(p => p.Colour.ToLower() == normalizedColour.ToLower());
        }

        return await query
            .OrderByDescending(p => p.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}
