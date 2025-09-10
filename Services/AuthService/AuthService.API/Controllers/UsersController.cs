using AuthService.Application.Features.Users.LoginUser;
using AuthService.Application.Features.Users.RefreshTokenUser;
using AuthService.Application.Features.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.Auth.Interfaces;
using Shared.Common.Interfaces;
using System.Security.Claims;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UsersController(IMediator mediator, IDateTimeProvider dateTimeProvider)
        {
            _mediator = mediator;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var response = await _mediator.Send(request);
            
            if (!response.IsSuccess)
                return Unauthorized(response);

            Response.Cookies.Append("refreshToken", response.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = _dateTimeProvider.UtcNow.AddDays(7)
            });

            return Ok(new
            {
                response.Value.Token
            });
        }

        [HttpPost(nameof(RefreshToken))]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Refresh token bulunamadı");

            var result = await _mediator.Send(new RefreshTokenUserRequest
            {
                RefreshToken = refreshToken
            });

            if (!result.IsSuccess)
                return Unauthorized(result.Error);

            Response.Cookies.Append("refreshToken", result.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new
            {
                result.Value.Token
            }); 
        }

        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response);

            Response.Cookies.Append("refreshToken", response.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = _dateTimeProvider.UtcNow.AddDays(7)   
            });

            return Ok(new
            {
                response.Value.AccessToken,
                response.Value.UserId,
                response.Value.Email,
                response.Value.FullName
            });
        }
    }
}
