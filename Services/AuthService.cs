using gym_management_api.DTO;
using gym_management_api.DTO.Authentication;
using gym_management_api.Interfaces;
using gym_management_api.Models;
using Microsoft.EntityFrameworkCore;

namespace gym_management_api.Services;

public class AuthService(ApplicationDbContext context, IJwtService jwtService, ILogger<AuthService> logger) : IAuthService
{
    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto, string deviceInfo = null)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshToken = jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            DeviceInfo = deviceInfo
        };

        context.RefreshTokens.Add(refreshTokenEntity);

        user.LastLoginAt = DateTime.UtcNow;
            
        await context.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = jwtService.GetTokenExpiration(),
            User = new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.GetType().Name,
                Role = user is Personnel personnel ? personnel.Role.ToString() : null
            }
        };
    }
    
    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenEntity = await context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (refreshTokenEntity == null || !refreshTokenEntity.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token");
        }

        var user = refreshTokenEntity.User;
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User account is inactive");
        }

        var newAccessToken = jwtService.GenerateAccessToken(user);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        refreshTokenEntity.IsRevoked = true;
        refreshTokenEntity.RevokedAt = DateTime.UtcNow;

        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            DeviceInfo = refreshTokenEntity.DeviceInfo
        };

        context.RefreshTokens.Add(newRefreshTokenEntity);
        await context.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = jwtService.GetTokenExpiration(),
            User = new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.GetType().Name,
                Role = user is Personnel personnel ? personnel.Role.ToString() : null
            }
        };
    }

    public async Task<bool> LogoutAsync(int userId, string refreshToken = null)
    {
        var query = context.RefreshTokens.Where(rt => rt.UserId == userId && !rt.IsRevoked);

        if (!string.IsNullOrEmpty(refreshToken))
        {
            query = query.Where(rt => rt.Token == refreshToken);
        }

        var tokens = await query.ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Current password is incorrect");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        await RevokeAllTokensAsync(userId);

        return true;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email && u.IsActive);

        if (user == null)
        {
            return true;
        }

        var existingTokens = await context.PasswordResetTokens
            .Where(prt => prt.UserId == user.Id && prt.IsActive)
            .ToListAsync();

        foreach (var token in existingTokens)
        {
            token.IsUsed = true;
            token.UsedAt = DateTime.UtcNow;
        }

        var resetToken = new PasswordResetToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };

        context.PasswordResetTokens.Add(resetToken);
        await context.SaveChangesAsync();

        //TODO: Send email with reset link
        logger.LogInformation($"Password reset token generated for user {user.Email}: {resetToken.Token}");

        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var resetToken = await context.PasswordResetTokens
            .Include(prt => prt.User)
            .FirstOrDefaultAsync(prt => prt.Token == resetPasswordDto.Token);

        if (resetToken == null || !resetToken.IsActive)
        {
            throw new BadRequestException("Invalid or expired reset token");
        }

        if (resetToken.User.Email != resetPasswordDto.Email)
        {
            throw new BadRequestException("Invalid reset token");
        }

        resetToken.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
        resetToken.User.UpdatedAt = DateTime.UtcNow;

        resetToken.IsUsed = true;
        resetToken.UsedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        await RevokeAllTokensAsync(resetToken.UserId);

        return true;
    }

    public async Task<bool> RevokeAllTokensAsync(int userId)
    {
        var tokens = await context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
        return true;
    }
}