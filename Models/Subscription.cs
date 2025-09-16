using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gym_management_api.Enums;

namespace gym_management_api.Models;

public class Subscription
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public virtual Member Member { get; set; }
    public SubscriptionTypeEnum Type { get; set; } = SubscriptionTypeEnum.Monthly;
        
    [Column(TypeName = "decimal(8,2)")]
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SubscriptionStatusEnum Status { get; set; } = SubscriptionStatusEnum.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    [StringLength(255)]
    public PaymentMethodEnum PaymentMethod { get; set; }
    public bool AutoRenewal { get; set; } = false;
    public bool IsCancelled { get; set; }
    public DateTime? CancelledAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}