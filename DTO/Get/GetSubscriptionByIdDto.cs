using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class GetSubscriptionByIdDto
{
    public int Id { get; set; }
    public SubscriptionTypeEnum SubscriptionType { get; set; }
    public SubscriptionStatusEnum Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public bool AutoRenewal { get; set; }
    
    public bool IsCancelled { get; set; }
    public DateTime? CancelledAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}