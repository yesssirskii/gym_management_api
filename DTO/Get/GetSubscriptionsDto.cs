using gym_management_api.Enums;
using gym_management_api.Models;

namespace gym_management_api.DTO.Get;

public class GetSubscriptionsDto
{
    public string UserName { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public SubscriptionStatusEnum Status { get; set; }
    public bool IsCancelled { get; set; }
    public SubscriptionTypeEnum SubscriptionType { get; set; }
}