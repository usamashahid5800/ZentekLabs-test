namespace Test.Services;

public interface IJwtTokenService
{
    string GenerateToken(string username, DateTime nowUtc, out DateTime expiresAtUtc);
}
