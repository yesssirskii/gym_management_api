using gym_management_api.DTO.Create;
using gym_management_api.Models;

namespace gym_management_api.Interfaces;

public interface ISubscriptionService
{
    Task<Subscription?> GetSubscriptionById(int id);
    Task<int> CreateSubscriptionAsync(CreateSubscriptionDto dto);
    Task<string> DeleteSubscription(int id);
}