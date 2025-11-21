using Shared.Auth.Models;

namespace Shared.Auth.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string email, List<string> roles);
        RefreshToken GenerateRefreshToken();
    }
}
