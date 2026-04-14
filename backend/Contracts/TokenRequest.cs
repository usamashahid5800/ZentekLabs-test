using System.ComponentModel.DataAnnotations;

namespace Test.Contracts;

public class TokenRequest
{
    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}
