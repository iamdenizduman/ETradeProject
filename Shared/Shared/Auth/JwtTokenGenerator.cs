using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Auth.Interfaces;
using Shared.Auth.Models;
using Shared.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Auth
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _settings;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JwtTokenGenerator(IOptions<JwtSettings> options, IDateTimeProvider dateTimeProvider)
        {
            _settings = options.Value;
            _dateTimeProvider = dateTimeProvider;
        }

        public string GenerateToken(Guid userId, string email, string role)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                 new Claim(JwtRegisteredClaimNames.Email, email),
                 new Claim(ClaimTypes.Role, role),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                notBefore: _dateTimeProvider.UtcNow,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(string email)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = _dateTimeProvider.UtcNow.AddDays(7)
            };
        }
    }
}