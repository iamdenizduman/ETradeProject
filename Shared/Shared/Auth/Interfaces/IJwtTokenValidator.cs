using System.Security.Claims;

namespace Shared.Auth.Interfaces
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromRefreshToken(string token);
    }
}
