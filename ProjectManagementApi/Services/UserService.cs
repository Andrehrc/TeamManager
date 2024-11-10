using Microsoft.IdentityModel.Tokens;
using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Repositories.Interfaces;
using ProjectManagementApi.Services.Interfaces;

namespace ProjectManagementApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEncryptDecryptService _encryptDecryptService;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IEncryptDecryptService encryptDecryptService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _encryptDecryptService = encryptDecryptService;
        }

        public async Task<(string, string)> SignUpAsync(string name, string email, string password)
        {
            var existingUser = await _userRepository.GetUserByEmail(email);
            if (!existingUser.IsNullOrEmpty())
            {
                throw new InvalidOperationException("User with this email already exists.");
            }
            var encryptedPassword = _encryptDecryptService.Encrypt(password);

            var user = new User
            {
                Name = name,
                Email = email,
                Password = encryptedPassword,
                ConfirmationToken = Guid.NewGuid(),
                EmailConfirmed = false
            };

            await _userRepository.CreateUserAsync(user);

            var token = await _tokenService.GenerateToken(user.Id.ToString());
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

            return (token, refreshToken);
        }
    }
}
