using System.ComponentModel.DataAnnotations;
using gym_management_api.Enums;

namespace gym_management_api.Models;
public abstract class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(11)]
    public long Oib { get; set; }
    
    [Required]
    [StringLength(50)]
    public required string Username { get; set; }
    
    [Required]
    [StringLength(255)]
    public required string Email { get; set; }
    
    [Required]
    public required string PasswordHash { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string FirstName { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string LastName { get; set; }
    
    [StringLength(50)]
    public required string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    [StringLength(10)]
    public GenderEnum Gender { get; set; }
    
    [Required]
    [StringLength(255)]
    public required string Address { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    [StringLength(255)]
    public string? ProfileImageUrl { get; set; }
    
    public DateTime? LastLoginAt { get; set; }
    public SubscriptionTypeEnum SubscriptionType { get; set; }
    
    // Navigation properties
    public virtual ICollection<Notification> SentNotifications { get; set; } = new List<Notification>();
    public virtual ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
}