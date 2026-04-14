using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Test.Contracts;

namespace Test.IntegrationTests;

public class ProductsApiTests : IClassFixture<ProductsApiFactory>
{
    private readonly HttpClient _client;

    public ProductsApiTests(ProductsApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_ShouldBeAnonymousAndReturnOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Products_ShouldRequireAuthentication()
    {
        var response = await _client.GetAsync("/products");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateAndListProducts_ShouldWorkForAuthenticatedClient()
    {
        var token = await GetAccessTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createRed = await _client.PostAsJsonAsync("/products", new CreateProductRequest
        {
            Name = "Red Mug",
            Colour = "Red",
            Price = 10.50m
        });

        var createBlue = await _client.PostAsJsonAsync("/products", new CreateProductRequest
        {
            Name = "Blue Cup",
            Colour = "Blue",
            Price = 8.75m
        });

        Assert.Equal(HttpStatusCode.Created, createRed.StatusCode);
        Assert.Equal(HttpStatusCode.Created, createBlue.StatusCode);

        var allProducts = await _client.GetFromJsonAsync<List<ProductResponse>>("/products");
        Assert.NotNull(allProducts);
        Assert.True(allProducts.Count >= 2);

        var blueProducts = await _client.GetFromJsonAsync<List<ProductResponse>>("/products?colour=blue");
        Assert.NotNull(blueProducts);
        Assert.All(blueProducts, p => Assert.Equal("blue", p.Colour, ignoreCase: true));
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var tokenResponse = await _client.PostAsJsonAsync("/auth/token", new TokenRequest
        {
            Username = "testuser",
            Password = "P@ssw0rd123"
        });

        tokenResponse.EnsureSuccessStatusCode();

        var payload = await tokenResponse.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(payload);
        Assert.False(string.IsNullOrWhiteSpace(payload.AccessToken));

        return payload.AccessToken;
    }
}
