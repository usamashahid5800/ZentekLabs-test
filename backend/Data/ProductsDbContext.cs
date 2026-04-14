using Microsoft.EntityFrameworkCore;
using Test.Domain;

namespace Test.Data;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();

        product.HasKey(p => p.Id);
        product.Property(p => p.Name).HasMaxLength(100).IsRequired();
        product.Property(p => p.Colour).HasMaxLength(40).IsRequired();
        product.Property(p => p.Price).HasColumnType("decimal(18,2)");
        product.Property(p => p.CreatedAtUtc).IsRequired();
    }
}
