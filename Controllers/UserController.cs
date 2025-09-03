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

    [HttpPost("create-new-user")]
    public async Task<ActionResult> CreateNewUser([FromForm] CreateUserDto userDto)
    {
        try
        {
            await userService.CreateUser(userDto);
            
            return Ok($"User created successfully with username {userDto.Username}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the user.");
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, [FromForm] UpdateUserDto dto)
    {
        var success = await userService.UpdateUserAsync(id, dto);
        
        if (!success)
            return NotFound("User not found.");
    
        return Ok("User updated successfully!");
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userService.DeleteUser(id);
        
            return Ok("User deleted successfully!");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the user.");
        }
    }
}