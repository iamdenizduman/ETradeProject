using AuthService.Application.Redis.Interfaces;
using AuthService.Application.Repositories.Interfaces;
using MediatR;
using Shared.Auth.Interfaces;
using Shared.Common;
using Shared.Common.Interfaces;

namespace AuthService.Application.Features.Users.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, Result<LoginUserResponse>>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IHashingHelper _hashingHelper;
        private readonly IUserRedisService _userRedisService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public LoginUserHandler(IUserReadRepository userReadRepository,
            IHashingHelper hashingHelper, IJwtTokenGenerator jwtTokenGenerator, IUserRedisService userRedisService, IDateTimeProvider dateTimeProvider)
        {
            _userReadRepository = userReadRepository;
            _hashingHelper = hashingHelper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRedisService = userRedisService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<LoginUserResponse>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userReadRepository.GetUserRoleByEmailAsync(request.Email);

                if (user == null)
                    return Result<LoginUserResponse>.Failure("Kullanıcı adı veya şifre hatalı");

                bool passwordCheck = _hashingHelper.VerifyPasswordHash(request.Password,
                    user.PasswordHash, user.PasswordSalt);

                if (!passwordCheck)
                    return Result<LoginUserResponse>.Failure("Kullanıcı adı veya şifre hatalı");

                var accessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.Email,
                        user?.UserOperationClaims?.FirstOrDefault()?.OperationClaim.Role);

                var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Email);

                var key = $"refreshToken:{refreshToken.Token}";
                var expiration = refreshToken.Expires - _dateTimeProvider.UtcNow;
                await _userRedisService.SetAsync(key, request.Email, expiration);

                return Result<LoginUserResponse>.Success(new LoginUserResponse
                {
                    Token = accessToken,
                    RefreshToken = refreshToken.Token
                });
            }
            catch (Exception ex)
            {
                return Result<LoginUserResponse>.Failure("Serviste hata meydana geldi");
            }
        }
    }
}