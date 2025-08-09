using Shared.Auth.Models;

namespace Shared.Auth.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string email, string role);
        RefreshToken GenerateRefreshToken(string email);
    }
}
