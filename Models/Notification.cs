using System.ComponentModel.DataAnnotations;
using gym_management_api.Enums;

namespace gym_management_api.Models;

public class Notification
{
    public int Id { get; set; }
        
    public int? SenderId { get; set; }
    public virtual User Sender { get; set; }
        
    public int RecipientId { get; set; }
    public virtual User Recipient { get; set; }
        
    [Required]
    [StringLength(255)]
    public string Title { get; set; }
        
    [Required]
    [StringLength(1000)]
    public string Message { get; set; }
        
    public NotificationTypeEnum Type { get; set; }
        
    public bool IsRead { get; set; } = false;
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
        
    [StringLength(255)]
    public string ActionUrl { get; set; }
}