using gym_management_api.Enums;

namespace gym_management_api.DTO.Create;

public class CreateSubscriptionDto
{
    public int MemberId { get; set; }
    public SubscriptionTypeEnum Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public SubscriptionStatusEnum Status { get; set; } = SubscriptionStatusEnum.Active;
    public PaymentMethodEnum PaymentMethod { get; set; }
    public bool AutoRenewal { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? CancelledAt { get; set; }
}