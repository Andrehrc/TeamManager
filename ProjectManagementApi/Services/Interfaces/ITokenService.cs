using ProjectManagementApi.Core.Models;
using System.Security.Claims;

namespace ProjectManagementApi.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(string userId);
        Task<User> ValidateUserAsync(string email, string password);
        Task<string> RefreshTokenAsync(string currentRefreshToken, int userId);
        Task<string> GenerateRefreshTokenAsync(int userId);
        Task DeleteRefreshTokenAsync(string refreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
