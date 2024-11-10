using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Core.Utils;
using ProjectManagementApi.Repositories.Interfaces;
using ProjectManagementApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProjectManagementApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;

        public TokenService(IOptions<JwtSettings> jwtSettings, IUserRepository userRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<string> GenerateToken(string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: creds
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return await Task.FromResult(tokenString);
        }

        public async Task<string> RefreshTokenAsync(string currentRefreshToken, int userId)
        {
            var savedRefreshToken = await _userRepository.GetRefreshTokenAsync(userId, currentRefreshToken);

            if (savedRefreshToken == null || savedRefreshToken.RefreshToken != currentRefreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token.");
            }

            var newRefreshToken = GenerateRefreshToken();
            savedRefreshToken.RefreshToken = newRefreshToken;
            savedRefreshToken.Expiration = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateRefreshTokenAsync(savedRefreshToken);

            return newRefreshToken;
        }

        public async Task<string> GenerateRefreshTokenAsync(int userId)
        {
            var refreshToken = new UserRefreshToken
            {
                RefreshToken = GenerateRefreshToken(),
                UserId = userId,
                Expiration = DateTime.UtcNow.AddDays(7)
            };

            await _userRepository.SaveRefreshTokenAsync(refreshToken);
            return refreshToken.RefreshToken;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task DeleteRefreshTokenAsync(string refreshToken)
        {
            await _userRepository.DeleteRefreshTokenAsync(refreshToken);
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            return await _userRepository.ValidateUserAsync(email, password);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido");
            }

            return principal;
        }
    }
}
