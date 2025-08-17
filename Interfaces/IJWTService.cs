using System.Security.Claims;
using gym_management_api.Models;

namespace gym_management_api.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    DateTime GetTokenExpiration();
}