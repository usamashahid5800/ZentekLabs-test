namespace Test.Contracts;

public class ProductResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Colour { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
