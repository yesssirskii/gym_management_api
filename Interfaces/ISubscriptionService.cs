using gym_management_api.DTO.Create;
using gym_management_api.DTO.Get;
using gym_management_api.DTO.Update;

namespace gym_management_api.Interfaces;

public interface ISubscriptionService
{
    Task<List<GetSubscriptionsDto>> GetSubscriptionsAsync();
    Task<GetSubscriptionByIdDto?> GetSubscriptionById(int id);
    Task<string> UpdateSubscriptionAsync(int userId, UpdateSubscriptionDto dto);
    Task<int> CreateSubscriptionAsync(CreateSubscriptionDto dto);
    Task<string> DeleteSubscription(int id);
}