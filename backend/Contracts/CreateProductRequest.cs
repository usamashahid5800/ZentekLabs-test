using System.ComponentModel.DataAnnotations;

namespace Test.Contracts;

public class CreateProductRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [MaxLength(40)]
    public string Colour { get; init; } = string.Empty;

    [Range(0.01, 1_000_000)]
    public decimal Price { get; init; }
}
