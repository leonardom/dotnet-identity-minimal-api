namespace Identity.API.Models;

public class TokenGenerationRequest
{
    public Guid UserId { get; init; }
    
    public string Email { get; init; }

    public Dictionary<string, object> CustomClaims { get; init; } = new();
}