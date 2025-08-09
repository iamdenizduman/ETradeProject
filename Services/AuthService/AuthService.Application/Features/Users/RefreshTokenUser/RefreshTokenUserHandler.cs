using AuthService.Application.Repositories.Interfaces;
using AuthService.Infrastructure.Redis.Interfaces;
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
            bool isExistRefreshToken = await _userRedisService.IsExistRefreshTokenByEmail(request.Email);
            if(!isExistRefreshToken)
                return Result<RefreshTokenUserResponse>.Failure("Refresh token geçersiz");

            var user = await _userReadRepository.GetUserRoleByEmailAsync(request.Email);
            if (user == null)
                return Result<RefreshTokenUserResponse>.Failure("Kullanıcı bulunamadı.");

            var newAccessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.Email,
                                 user?.UserOperationClaims?.FirstOrDefault()?.OperationClaim.Role);

            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Email);

            var key = $"refreshToken:{user.Email}";
            var expiration = newRefreshToken.Expires - _dateTimeProvider.UtcNow;

            await _userRedisService.SetAsync(key, newRefreshToken.Token, expiration);

            return Result<RefreshTokenUserResponse>.Success(new RefreshTokenUserResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }
    }
}
