namespace Shared.Auth.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        List<string> Roles { get; }
        bool IsAuthenticated { get; }
    }
}
