using gym_management_api.Enums;
using gym_management_api.Models;

namespace gym_management_api.DTO.Get;

public class GetSubscriptionsDto
{
    public int Id { get; set; }
    public string? MembershipNumber { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public bool AutoRenewal { get; set; }
    public SubscriptionStatusEnum Status { get; set; }
    public bool IsCancelled { get; set; }
    public SubscriptionTypeEnum SubscriptionType { get; set; }
}