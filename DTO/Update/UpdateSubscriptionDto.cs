using gym_management_api.Enums;

namespace gym_management_api.DTO.Update;

public class UpdateSubscriptionDto
{
    public int Id { get; set; }
    public SubscriptionTypeEnum SubscriptionType { get; set; }
    public SubscriptionStatusEnum Status { get; set; }
    public decimal Price { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool AutoRenewal { get; set; }
}