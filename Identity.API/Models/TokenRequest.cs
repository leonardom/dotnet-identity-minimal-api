namespace Identity.API.Models;

public class TokenRequest
{
    public Guid UserId { get; init; }
    
    public string Email { get; init; }

    public Dictionary<string, object> CustomClaims { get; init; } = new();
}