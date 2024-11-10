using Microsoft.EntityFrameworkCore;
using ProjectManagementApi.Contexts;
using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Repositories.Interfaces;
using ProjectManagementApi.Services.Interfaces;

namespace ProjectManagementApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IEncryptDecryptService _encryptDecryptService;

        public UserRepository(ApplicationDbContext dbContext, IEncryptDecryptService encryptDecryptService)
        {
            _context = dbContext;
            _encryptDecryptService = encryptDecryptService;
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            var users = await GetUserByEmail(email);

            User user = users.Find(w => w.Password == _encryptDecryptService.Encrypt(password));

            return user;
        }

        public async Task<List<User>> GetUserByEmail(string email)
        {
            return await _context.Users.Where(w => w.Email == email).ToListAsync();

        }

        public async Task SaveRefreshTokenAsync(UserRefreshToken refreshToken)
        {
            var exists = await _context.RefreshTokens.Where(w => w.UserId == refreshToken.UserId).FirstOrDefaultAsync();

            if (exists != null)
            {
                exists.RefreshToken = refreshToken.RefreshToken;
                exists.Expiration = refreshToken.Expiration;
            }
            else
            {
                await _context.RefreshTokens.AddAsync(refreshToken);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<UserRefreshToken> GetRefreshTokenAsync(int userId, string refreshToken)
        {
            return await _context.RefreshTokens.SingleOrDefaultAsync(x =>
                x.UserId == userId && x.RefreshToken == refreshToken);
        }

        public async Task DeleteRefreshTokenAsync(string refreshToken)
        {
            await _context.RefreshTokens.Where(t => t.RefreshToken == refreshToken).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRefreshTokenAsync(UserRefreshToken refreshToken)
        {
            var existingToken = await _context.RefreshTokens
                .FindAsync(refreshToken.Id);

            if (existingToken == null)
            {
                throw new KeyNotFoundException("Refresh token not found.");
            }

            existingToken.RefreshToken = refreshToken.RefreshToken;
            existingToken.Expiration = refreshToken.Expiration;

            _context.RefreshTokens.Update(existingToken);
            await _context.SaveChangesAsync();
        }
    }
}
