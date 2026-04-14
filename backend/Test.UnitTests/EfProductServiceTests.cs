using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Services;

namespace Test.UnitTests;

public class EfProductServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldStoreProductWithTrimmedValues()
    {
        await using var dbContext = CreateDbContext();
        var service = new EfProductService(dbContext);

        var created = await service.CreateAsync("  Laptop  ", "  Blue  ", 999.99m);

        Assert.Equal("Laptop", created.Name);
        Assert.Equal("Blue", created.Colour);
        Assert.Equal(999.99m, created.Price);

        var all = await service.GetAllAsync();
        Assert.Single(all);
    }

    [Fact]
    public async Task GetAllAsync_ShouldFilterByColourCaseInsensitively()
    {
        await using var dbContext = CreateDbContext();
        var service = new EfProductService(dbContext);

        await service.CreateAsync("Phone", "Red", 100m);
        await service.CreateAsync("Tablet", "BLUE", 300m);
        await service.CreateAsync("Monitor", "blue", 250m);

        var filtered = await service.GetAllAsync("Blue");

        Assert.Equal(2, filtered.Count);
        Assert.All(filtered, p => Assert.Equal("blue", p.Colour, ignoreCase: true));
    }

    private static ProductsDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase($"ProductsUnitTests-{Guid.NewGuid()}")
            .Options;

        return new ProductsDbContext(options);
    }
}
