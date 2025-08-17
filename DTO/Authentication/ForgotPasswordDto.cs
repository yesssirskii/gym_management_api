using System.ComponentModel.DataAnnotations;

namespace gym_management_api.DTO.Authentication;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}