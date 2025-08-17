using System.ComponentModel.DataAnnotations;

namespace gym_management_api.DTO.Authentication;

public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; }
        
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; }
}