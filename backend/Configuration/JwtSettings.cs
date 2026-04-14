namespace Test.Configuration;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; } = 60;

    public string TestUsername { get; init; } = string.Empty;
    public string TestPassword { get; init; } = string.Empty;
}
