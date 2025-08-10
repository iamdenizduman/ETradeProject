using MediatR;
using Shared.Common;

namespace AuthService.Application.Features.Users.RegisterUser
{
    public class RegisterUserRequest : IRequest<Result<RegisterUserResponse>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
