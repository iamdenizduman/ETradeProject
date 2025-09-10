using AuthService.Application.Repositories.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Redis.Interfaces;
using MediatR;
using Shared.Auth.Enums;
using Shared.Auth.Interfaces;
using Shared.Common;
using Shared.Common.Interfaces;
using Shared.Persistence.Interfaces.EFCore;

namespace AuthService.Application.Features.Users.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, Result<RegisterUserResponse>>
    {
        readonly IUserReadRepository _userReadRepository;
        readonly IUserWriteRepository _userWriteRepository;
        readonly IOperationClaimReadRepository _operationClaimReadRepository;
        readonly IUserOperationClaimWriteRepository _userOperationClaimWriteRepository;
        readonly IHashingHelper _hashingHelper;
        readonly IUnitOfWork _unitOfWork;
        readonly IJwtTokenGenerator _jwtTokenGenerator;
        readonly IDateTimeProvider _dateTimeProvider;
        readonly IUserRedisService _userRedisService;

        public RegisterUserHandler(IUserWriteRepository userWriteRepository, IHashingHelper hashingHelper, IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator, IDateTimeProvider dateTimeProvider, IUserReadRepository userReadRepository, IUserOperationClaimWriteRepository userOperationClaimWriteRepository, IOperationClaimReadRepository operationClaimReadRepository, IUserRedisService userRedisService)
        {
            _userWriteRepository = userWriteRepository;
            _hashingHelper = hashingHelper;
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _dateTimeProvider = dateTimeProvider;
            _userReadRepository = userReadRepository;
            _userOperationClaimWriteRepository = userOperationClaimWriteRepository;
            _operationClaimReadRepository = operationClaimReadRepository;
            _userRedisService = userRedisService;
        }

        public async Task<Result<RegisterUserResponse>> Handle(RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var user = (await _userReadRepository.GetAllAsync()).Where(u => u.Email == request.Email)?.FirstOrDefault();

                if (user != null)
                    return Result<RegisterUserResponse>.Failure("Bu email zaten sistemde kayıtlı");

                _hashingHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                
                var newUser = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                await _userWriteRepository.AddAsync(newUser);

                var customerClaim = (await _operationClaimReadRepository.GetAllAsync())
                                    .FirstOrDefault(oc => oc.Role == RoleType.Customer.ToString());

                if (customerClaim == null)
                    return Result<RegisterUserResponse>.Failure("Müşteri rolü bulunamadı.");

                await _userOperationClaimWriteRepository.AddAsync(new UserOperationClaim
                {
                    UserId = newUser.Id,
                    OperationClaimId = customerClaim.Id
                });

                var accessToken = _jwtTokenGenerator.GenerateToken(newUser.Id, newUser.Email,
                        RoleType.Customer.ToString());

                var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(newUser.Email);

                var key = $"refreshToken:{refreshToken.Token}";
                var expiration = refreshToken.Expires - _dateTimeProvider.UtcNow;
                await _userRedisService.SetAsync(key, newUser.Email, expiration);

                await _unitOfWork.SaveChangesAsync();
                
                return Result<RegisterUserResponse>.Success(new RegisterUserResponse
                {
                    AccessToken = accessToken,
                    UserId= newUser.Id,
                    Email= newUser.Email,
                    FullName = newUser.FirstName + " " + newUser.LastName,
                    RefreshToken = refreshToken.Token
                });
            }
            catch (Exception ex)
            {
                return Result<RegisterUserResponse>.Failure("Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }
        }
    }
}
