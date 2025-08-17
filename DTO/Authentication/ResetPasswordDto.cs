using System.ComponentModel.DataAnnotations;

namespace gym_management_api.DTO.Authentication;

public class ResetPasswordDto
{
    [Required]
    public string Token { get; set; }
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }
        
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; }
}