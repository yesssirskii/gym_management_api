using gym_management_api.DTO.Authentication;
using gym_management_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace gym_management_api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromForm] LoginDto loginDto)
        {
            try
            {
                var deviceInfo = Request.Headers.UserAgent.ToString();
                var response = await _authService.LoginAsync(loginDto, deviceInfo);
                
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "An error occurred during login" });
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromForm] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "An error occurred during token refresh" });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout([FromForm] RefreshTokenDto refreshTokenDto = null)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _authService.LogoutAsync(userId, refreshTokenDto?.RefreshToken);
                
                return Ok(new { Message = "Logged out successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "An error occurred during logout" });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromForm] ChangePasswordDto changePasswordDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _authService.ChangePasswordAsync(userId, changePasswordDto);
                
                return Ok(new { Message = "Password changed successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "An error occurred while changing password" });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserInfoDto>> GetCurrentUser()
        {
            var userInfo = new UserInfoDto
            {
                Id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
                Email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                FirstName = User.FindFirst("FirstName")?.Value ?? string.Empty,
                LastName = User.FindFirst("LastName")?.Value ?? string.Empty,
                UserType = User.FindFirst("UserType")?.Value ?? string.Empty,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty
            };

            return Ok(userInfo);
        }

        [HttpPost("revoke-all-tokens")]
        [Authorize]
        public async Task<ActionResult> RevokeAllTokens()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _authService.RevokeAllTokensAsync(userId);
                
                return Ok(new { Message = "All tokens revoked successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Error = "An error occurred while revoking tokens" });
            }
        }
}