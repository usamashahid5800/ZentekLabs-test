using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Test.Configuration;
using Test.Contracts;
using Test.Services;

namespace Test.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _settings;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IOptions<JwtSettings> settings, IJwtTokenService jwtTokenService)
    {
        _settings = settings.Value;
        _jwtTokenService = jwtTokenService;
    }

    [AllowAnonymous]
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CreateToken([FromBody] TokenRequest request)
    {
        var isValidUser = string.Equals(request.Username, _settings.TestUsername, StringComparison.Ordinal)
            && string.Equals(request.Password, _settings.TestPassword, StringComparison.Ordinal);

        if (!isValidUser)
        {
            return Unauthorized();
        }

        var accessToken = _jwtTokenService.GenerateToken(request.Username, DateTime.UtcNow, out var expiresAtUtc);

        return Ok(new TokenResponse
        {
            AccessToken = accessToken,
            ExpiresAtUtc = expiresAtUtc
        });
    }
}
