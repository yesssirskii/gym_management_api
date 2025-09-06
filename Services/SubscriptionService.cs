using gym_management_api.DTO.Create;
using gym_management_api.DTO.Get;
using gym_management_api.DTO.Update;
using gym_management_api.Enums;
using gym_management_api.Models;
using Microsoft.EntityFrameworkCore;

namespace gym_management_api.Services;

public class SubscriptionService(ApplicationDbContext dbContext)
{
    public async Task<int> CreateSubscriptionAsync(CreateSubscriptionDto dto)
    {
        var endDate = dto.Type switch
        {
            SubscriptionTypeEnum.Daily => dto.StartDate.AddDays(1),
            SubscriptionTypeEnum.Monthly => dto.StartDate.AddMonths(1),
            SubscriptionTypeEnum.Yearly => dto.StartDate.AddYears(1),
            _ => throw new ArgumentException("Invalid subscription type")
        };
        
        var subscription = new Subscription()
        {
            MemberId = dto.MemberId,
            Type = dto.Type,
            StartDate = dto.StartDate, 
            EndDate = endDate,
            Price = dto.Price,
            CreatedAt = DateTime.UtcNow,
            Status = dto.Status,
            PaymentMethod = dto.PaymentMethod,
            AutoRenewal = dto.AutoRenewal,
            IsCancelled = false,
            CancelledAt = null
        };

        dbContext.Subscriptions.Add(subscription);
        await dbContext.SaveChangesAsync();
        
        return subscription.Id;
    }

    public async Task<GetSubscriptionByIdDto?> GetSubscriptionByMemberId(int memberId)
    {
        return await dbContext.Subscriptions
            .Where(s => s.MemberId == memberId && s.IsDeleted == false)
            .Select(s => new GetSubscriptionByIdDto
            {
                Id = s.Id,
                SubscriptionType = s.Type,
                Status = s.Status,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Price = s.Price,
                PaymentMethod = s.PaymentMethod,
                AutoRenewal = s.AutoRenewal
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<GetSubscriptionsDto>> GetSubscriptionsAsync()
    {
        return await dbContext.Subscriptions
            .Include(s => s.Member)
            .Where(s => s.IsDeleted == false)
            .Select(s => new GetSubscriptionsDto
            {
                UserName = s.Member.Username,
                EndDate = s.EndDate,
                Price = s.Price,
                Status = s.Status,
                IsCancelled = s.IsCancelled,
                SubscriptionType = s.Type
            })
            .ToListAsync();
    }

    public async Task<string> UpdateSubscriptionAsync(int userId, UpdateSubscriptionDto dto)
    {
        var subscriptionToUpdate = await dbContext.Subscriptions
            .FirstOrDefaultAsync(s => s.MemberId == userId);
        
        if (subscriptionToUpdate == null)
        {
            return "Subscription with id " + dto.Id + " does not exist.";
        }

        subscriptionToUpdate.Type = dto.SubscriptionType;
        subscriptionToUpdate.Status = dto.Status;
        subscriptionToUpdate.PaymentMethod = dto.PaymentMethod;
        subscriptionToUpdate.AutoRenewal = dto.AutoRenewal;
        
        await dbContext.SaveChangesAsync();
        
        return "Subscription with id " + dto.Id + " has been successfully updated.";
    }

    public async Task<string> DeleteSubscription(int id)
    {
        var subscriptionToDelete =  await dbContext.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (subscriptionToDelete == null)
        {
            return "Subscription with id " + id + " does not exist.";
        }

        subscriptionToDelete.AutoRenewal = false;
        subscriptionToDelete.IsCancelled = true;
        subscriptionToDelete.CancelledAt = DateTime.UtcNow;
        subscriptionToDelete.Status = SubscriptionStatusEnum.Cancelled;
        subscriptionToDelete.EndDate = DateTime.UtcNow;

        subscriptionToDelete.IsDeleted = true;
        subscriptionToDelete.DeletedAt = DateTime.UtcNow;
        
        dbContext.Subscriptions.Update(subscriptionToDelete);
        await dbContext.SaveChangesAsync();

        return "Subscription with id " + id + " deleted.";
    }
}