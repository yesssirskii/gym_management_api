using System.ComponentModel.DataAnnotations;

namespace gym_management_api.DTO.Authentication;

public class LoginDto
{
    [Required]
    public string Username { get; set; }
        
    [Required]
    public string Password { get; set; }
}