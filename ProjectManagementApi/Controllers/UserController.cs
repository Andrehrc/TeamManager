using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Core.Requests;
using ProjectManagementApi.Repositories.Interfaces;
using ProjectManagementApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProjectManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public UserController(ITokenService tokenService, IUserRepository userRepository, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Login([FromBody] SignUpRequest userRequest)
        {
            try
            {
                var token = await _userService.SignUpAsync(userRequest.Name, userRequest.Email, userRequest.Password);

                return Ok(new
                {
                    Token = token.token,
                    RefreshToken = token.refreshToken
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            var user = await _tokenService.ValidateUserAsync(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = await _tokenService.GenerateToken(user.Id.ToString());
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(tokenRequest.Token);
                var claimsIdentity = principal.Identity as ClaimsIdentity;

                var userIdClaim = claimsIdentity?.FindFirst(JwtRegisteredClaimNames.Sub);
                if (userIdClaim == null)
                {
                    return Unauthorized("Invalid token claims.");
                }

                var userId = userIdClaim.Value;

                var newRefreshToken = await _tokenService.RefreshTokenAsync(tokenRequest.RefreshToken, int.Parse(userId));

                var newAccessToken = await _tokenService.GenerateToken(userId.ToString());

                return Ok(new
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Invalid refresh token.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromQuery] string tokenRequest)
        {
            await _tokenService.DeleteRefreshTokenAsync(tokenRequest);
            return Ok(new { Message = "Logout successful." });
        }
    }
}
