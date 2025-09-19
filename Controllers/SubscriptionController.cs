using gym_management_api.DTO.Create;
using gym_management_api.DTO.Update;
using gym_management_api.Models;
using gym_management_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace gym_management_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController(SubscriptionService subscriptionService) : ControllerBase
{
    /// <summary>
    /// Returns all subscriptions for the subscriptions table.
    /// </summary>
    /// <returns></returns>
    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetSubscriptions()
    {
        try
        {
            var subscriptions = await subscriptionService.GetSubscriptionsAsync();
            
            return Ok(subscriptions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error while fetching the subscriptions: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Returns a specific subscription for the given user id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetSubscriptionByUserId(int userId)
    {
        try
        {
            var subscription = await subscriptionService.GetSubscriptionByMemberId(userId);
            
            return Ok(subscription);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while fetching subscription; {ex.Message}");
        }
    }
    
    /// <summary>
    /// Creates a new subscription for the given user id.
    /// </summary>
    /// <param name="subscription"></param>
    /// <returns></returns>
    [HttpPost("new-subscription")]
    public async Task<IActionResult> CreateSubscription([FromForm] CreateSubscriptionDto subscription)
    {
        try
        {
            await subscriptionService.CreateSubscriptionAsync(subscription);

            return Ok(new { message = $"Subscription for member {subscription.MemberId} created successfully.", success = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while creating subscription; {ex.Message}");       
        }
    }
    
    /// <summary>
    /// Updates a subscription for the given user based on the id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="subscription"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubscription(int id, [FromBody] UpdateSubscriptionDto subscription)
    {
        try
        {
            await subscriptionService.UpdateSubscriptionAsync(id, subscription);

            return Ok(new { message = $"Subscription for member {id} updated successfully.", success = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating subscription; {ex.Message}");       
        }
    }
    
    /// <summary>
    /// Updates a user's subscription based on the id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="subscription"></param>
    /// <returns></returns>
    [HttpPut("renew/{id}")]
    public async Task<IActionResult> RenewSubscription(int id, [FromBody] UpdateSubscriptionDto subscription)
    {
        try
        {
            var renewedSubscription = await subscriptionService.RenewSubscriptionAsync(id, subscription);

            if (renewedSubscription == null)
            {
                return BadRequest("Subscription not found.");
            }

            return Ok(new { message = $"Subscription for member {id} renewed successfully.", success = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating subscription; {ex.Message}");       
        }
    }
    
    /// <summary>
    /// Deletes a subscription for the given user id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("subscription/{id}")]
    public async Task<IActionResult> DeleteSubscription(int id)
    {
        try
        {
            await subscriptionService.DeleteSubscription(id);
            
            return Ok("Subscription deleted successfully!");       
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting subscription; {ex.Message}");
        }
    }
}