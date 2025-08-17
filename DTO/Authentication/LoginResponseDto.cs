using gym_management_api.DTO.Update;

namespace gym_management_api.DTO.Authentication;

public class LoginResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public UserInfoDto User { get; set; }
}