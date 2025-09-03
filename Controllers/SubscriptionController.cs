using gym_management_api.DTO.Create;
using gym_management_api.Models;
using gym_management_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace gym_management_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController(SubscriptionService subscriptionService) : ControllerBase
{
    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetSubscriptions()
    {
        try
        {
            var subscriptions = await subscriptionService.GetSubscriptionsAsync();
            
            return Ok(subscriptions);
        }
        catch (Exception)
        {
            return StatusCode(500, "$Error while fetching the subscriptions: {ex.Message}");
        }
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetSubscriptionByUserId(int userId)
    {
        try
        {
            var subscription = await subscriptionService.GetSubscriptionByMemberId(userId);
            
            return Ok(subscription);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching subscription.");
        }
    }
    
    [HttpPost("new-subscription")]
    public async Task<IActionResult> CreateSubscription([FromForm] CreateSubscriptionDto subscription)
    {
        try
        {
            await subscriptionService.CreateSubscriptionAsync(subscription);

            return Ok("Subscription created successfully!");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating subscription.");       
        }
    }
    
    [HttpDelete("subscription/{id}")]
    public async Task<IActionResult> DeleteSubscription(int id)
    {
        try
        {
            await subscriptionService.DeleteSubscription(id);
            
            return Ok("Subscription deleted successfully!");       
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting subscription.");
        }
    }
}