using System.ComponentModel.DataAnnotations;

namespace gym_management_api.DTO.Authentication;

public class RefreshTokenDto
{
    [Required]
    public string RefreshToken { get; set; }
}