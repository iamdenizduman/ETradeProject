using MediatR;
using Shared.Common;

namespace AuthService.Application.Features.Users.RefreshTokenUser
{
    public class RefreshTokenUserRequest : IRequest<Result<RefreshTokenUserResponse>>
    {
        public string Email { get; set; }
    }
}
