using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Services;

public class IdentifyService(IConfiguration config) : IIdentifyService
{
    public string GenerateToken(string email, Guid userId, Dictionary<string, object> customClaims)
    {
        var jwtKey = config.GetValue<string>("Jwt:Key")!;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtKey);
        var claims = CreateClaims(email, userId, customClaims);
        var tokenLifetime = TimeSpan.FromMinutes(config.GetValue<int>("Jwt:LifetimeInMinutes"));
        var tokenDescriptor = CreateSecurityTokenDescriptor(claims, tokenLifetime, key);
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor CreateSecurityTokenDescriptor(List<Claim> claims, TimeSpan tokenLifetime,
        byte[] key)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(tokenLifetime),
            Issuer = config.GetValue<string>("Jwt:Issuer")!,
            Audience = config.GetValue<string>("Jwt:Audience")!,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenDescriptor;
    }

    private static List<Claim> CreateClaims(string email, Guid userId, Dictionary<string, object> customClaims)
    {
        var claims = CreateDefaultClaims(email, userId);
        AddCustomClaims(claims, customClaims);
        return claims;
    }

    private static List<Claim> CreateDefaultClaims(string email, Guid userId)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("userid", userId.ToString())
        ];
    }

    private static void AddCustomClaims(List<Claim> claims, Dictionary<string, object> customClaims)
    {
        claims.AddRange(from claimPair in customClaims
            let jsonElement = (JsonElement)claimPair.Value
            let valueType = jsonElement.ValueKind switch
            {
                JsonValueKind.True => ClaimValueTypes.Boolean,
                JsonValueKind.False => ClaimValueTypes.Boolean,
                JsonValueKind.Number => ClaimValueTypes.Double,
                _ => ClaimValueTypes.String
            }
            select new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType));
    }
}