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
            Console.WriteLine($"Exception: {ex.Message}");
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        
            // If it's a DbUpdateException, get more details
            if (ex is Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                Console.WriteLine($"DB Update Exception: {dbEx.InnerException?.Message}");
            }

            throw;
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<int>> UpdateUser(int id, [FromForm] UpdateUserDto dto)
    {
        try
        {
            var updateUser = await userService.UpdateUserAsync(id, dto);

            if (updateUser == null || updateUser == 0)
                return NotFound("User not found.");

            return Ok(updateUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
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