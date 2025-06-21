namespace Identity.API.Services;

public interface IIdentifyService
{
    string GenerateToken(string email, Guid userId, Dictionary<string, object> claims);
}