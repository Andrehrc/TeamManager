using ProjectManagementApi.Core.Models;

namespace ProjectManagementApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<User> ValidateUserAsync(string email, string password);
        Task<List<User>> GetUserByEmail(string email);
        Task SaveRefreshTokenAsync(UserRefreshToken refreshToken);
        Task<UserRefreshToken> GetRefreshTokenAsync(int userId, string refreshToken);
        Task DeleteRefreshTokenAsync(string refreshToken);
        Task UpdateRefreshTokenAsync(UserRefreshToken refreshToken);
    }
}
