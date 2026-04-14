using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Contracts;
using Test.Services;

namespace Test.Controllers;

[ApiController]
[Route("products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var created = await _productService.CreateAsync(request.Name, request.Colour, request.Price, cancellationToken);

        return CreatedAtAction(nameof(GetAll), new
        {
            colour = created.Colour
        }, ToResponse(created));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? colour = null, CancellationToken cancellationToken = default)
    {
        var products = (await _productService.GetAllAsync(colour, cancellationToken))
            .Select(ToResponse)
            .ToList();

        return Ok(products);
    }

    private static ProductResponse ToResponse(Domain.Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Colour = product.Colour,
            Price = product.Price,
            CreatedAtUtc = product.CreatedAtUtc
        };
    }
}
