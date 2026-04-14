using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Test.Data;

namespace Test.IntegrationTests;

public class ProductsApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var dbName = $"ProductsIntegrationTests-{Guid.NewGuid()}";

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ProductsDbContext>>();
            services.AddDbContext<ProductsDbContext>(options =>
                options.UseInMemoryDatabase(dbName));
        });
    }
}
