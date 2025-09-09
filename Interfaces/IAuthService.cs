using gym_management_api.DTO.Authentication;

namespace gym_management_api.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto, string deviceInfo = null);
    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(int userId, string refreshToken = null);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    Task<bool> RevokeAllTokensAsync(int userId);
}