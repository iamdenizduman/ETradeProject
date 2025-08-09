using MediatR;
using Shared.Common;

namespace AuthService.Application.Features.Users.LoginUser
{
    public class LoginUserRequest : IRequest<Result<LoginUserResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}