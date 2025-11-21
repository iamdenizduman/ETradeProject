using AuthService.Application.Redis.Interfaces;
using AuthService.Application.Repositories.Interfaces;
using MediatR;
using Shared.Auth.Interfaces;
using Shared.Common;
using Shared.Common.Interfaces;

namespace AuthService.Application.Features.Users.RefreshTokenUser
{
    public class RefreshTokenUserHandler : IRequestHandler<RefreshTokenUserRequest, Result<RefreshTokenUserResponse>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRedisService _userRedisService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RefreshTokenUserHandler(IJwtTokenGenerator jwtTokenGenerator,
            IUserReadRepository userReadRepository, IUserRedisService userRedisService, IDateTimeProvider dateTimeProvider)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userReadRepository = userReadRepository;
            _userRedisService = userRedisService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<RefreshTokenUserResponse>> Handle(RefreshTokenUserRequest request,
            CancellationToken cancellationToken)
        {
            string email = await _userRedisService.GetEmailByRefreshToken(request.RefreshToken);
            if(string.IsNullOrEmpty(email))
                return Result<RefreshTokenUserResponse>.Failure("Refresh token geçersiz");

            var user = await _userReadRepository.GetUserRoleByEmailAsync(email);
            if (user == null)
                return Result<RefreshTokenUserResponse>.Failure("Kullanıcı bulunamadı.");

            var newAccessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.Email,
                                 user?.UserOperationClaims?.Select(uoc => uoc.OperationClaim.Role).ToList());

            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var key = $"refreshToken:{refreshToken.Token}";
            var expiration = refreshToken.Expires - _dateTimeProvider.UtcNow;

            await _userRedisService.SetAsync(key, email, expiration);

            return Result<RefreshTokenUserResponse>.Success(new RefreshTokenUserResponse
            {
                Token = newAccessToken,
                RefreshToken = refreshToken.Token
            });
        }
    }
}
