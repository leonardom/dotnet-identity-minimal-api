using Identity.API.Models;
using Identity.API.Services;

namespace Identity.API.Routes;

public static class IdentityRoutes
{
    public static void AddRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost("/token", (TokenRequest request, IIdentifyService identifyService) =>
            {
                var email = request.Email;
                var userId = request.UserId;
                var customClaims = request.CustomClaims;
                var token = identifyService.GenerateToken(email, userId, customClaims);
                var response = new TokenResponse
                {
                    Token = token
                };
                return Results.Ok(response);
            }
        );
    }
}