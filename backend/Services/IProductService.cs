using Test.Domain;

namespace Test.Services;

public interface IProductService
{
    Task<Product> CreateAsync(string name, string colour, decimal price, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Product>> GetAllAsync(string? colour = null, CancellationToken cancellationToken = default);
}
