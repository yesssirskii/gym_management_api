using gym_management_api.Services;
using Microsoft.AspNetCore.Mvc;
using gym_management_api.DTO.Create;
using gym_management_api.DTO.Get;
using gym_management_api.DTO.Update;

namespace gym_management_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService) : ControllerBase
{
    /// <summary>
    /// Returns all GetUsersDto objects for the user tables.
    /// </summary>
    /// <returns></returns>
    [HttpGet("user-table-data")]
    public async Task<ActionResult<List<GetUsersDto>>> GetUserDataForTable()
    {
        try 
        {
            var users = await userService.GetUserDataForTable();
            return Ok(users);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching users");
        }
    }
    
    /// <summary>
    /// Gets a user by their id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<GetUsersDto>>> GetUserById(int userId)
    {
        try 
        {
            var user = await userService.GetUserById(userId);
            return Ok(user);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error while fetching data for the user.");
        }
    }
    
    /// <summary>
    /// Returns all users whcich have the UserType of Member.
    /// </summary>
    /// <returns></returns>
    [HttpGet("members")]
    public async Task<ActionResult<List<GetUsersDto>>> GetAllMembers()
    {
        try 
        {
            var members = await userService.GetMembers();
            
            return Ok(members);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occured while fetching members.");
        }
    }
    
    /// <summary>
    /// Returns all users whcich have the UserType of Trainer.
    /// </summary>
    /// <returns></returns>
    [HttpGet("trainers")]
    public async Task<ActionResult<List<GetUsersDto>>> GetAllTrainers()
    {
        try 
        {
            var trainers = await userService.GetTrainers();
            
            return Ok(trainers);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occured while fetching trainers.");
        }
    }
    
    /// <summary>
    /// Returns all users whcich have the UserType of Personnel.
    /// </summary>
    /// <returns></returns>
    [HttpGet("personnel")]
    public async Task<ActionResult<List<GetPersonnelDto>>> GetAllPersonnel()
    {
        try 
        {
            var personnel = await userService.GetPersonnel();
            
            return Ok(personnel);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occured while fetching personnel.");
        }
    }

    /// <summary>
    /// Creates a new User entity object.
    /// </summary>
    /// <param name="userDto"></param>
    /// <returns></returns>
    [HttpPost("new")]
    public async Task<ActionResult> CreateNewUser([FromBody] CreateUserDto userDto)
    {
        try
        {
            await userService.CreateUserAsync(userDto);
            
            return Ok(new { 
                message = $"User created successfully with username {userDto.Username}",
                success = true
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occured while updating the user; {ex?.Message}");       
        }
    }
    
    /// <summary>
    /// Updates a user entity object and subscription if applicable.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isSubscriptionUpdate"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id}/{isSubscriptionUpdate:bool}")]
    public async Task<ActionResult<int>> UpdateUser(int id, bool isSubscriptionUpdate,[FromBody] UpdateUserDto dto)
    {
        try
        {
            var updateUser = await userService.UpdateUserAsync(id, isSubscriptionUpdate, dto);

            if (updateUser == null || updateUser == 0)
                return NotFound("User not found.");

            return Ok(updateUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
    
    /// <summary>
    /// Soft-deletes a user by their id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userService.DeleteUser(id);
        
            return Ok("User deleted successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the user; {ex.Message}");
        }
    }
}