using System.ComponentModel.DataAnnotations;

namespace gym_management_api.Models;

public class RefreshToken
{
    public int Id { get; set; }
        
    [Required]
    public string Token { get; set; }
        
    public int UserId { get; set; }
    public virtual User User { get; set; }
        
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; } = false;
    public DateTime? RevokedAt { get; set; }
        
    [StringLength(500)]
    public string DeviceInfo { get; set; }
        
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}