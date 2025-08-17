using System.ComponentModel.DataAnnotations;
using gym_management_api.Models;

namespace gym_management_api.Models;

public class PasswordResetToken
{
    public int Id { get; set; }
        
    [Required]
    public string Token { get; set; }
        
    public int UserId { get; set; }
    public virtual User User { get; set; }
        
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsUsed { get; set; } = false;
    public DateTime? UsedAt { get; set; }
        
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsUsed && !IsExpired;
}