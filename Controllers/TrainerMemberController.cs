using gym_management_api.DTO;
using gym_management_api.DTO.Create;
using gym_management_api.DTO.Get;
using gym_management_api.DTO.Update;
using gym_management_api.Interfaces;
using gym_management_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace gym_management_api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TrainersController(TrainerMemberService trainerMemberService) : ControllerBase
{
    [HttpGet("trainers")]
    public async Task<ActionResult<List<GetTrainersDto>>> CreateTGetAllTrainers()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var trainers = await trainerMemberService.GetAllTrainersAsync();

            return Ok(trainers);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Error = "An error occurred while fetching trainers" });
        }
    }
    
    [HttpGet("trainers/{id}")]
    public async Task<ActionResult<List<GetTrainersDto>>> GetTrainerById(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var trainer = await trainerMemberService.GetTrainerByIdAsync(id);

            return Ok(trainer);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Error = "An error occurred while fetching trainer data" });
        }
    }
    
    [HttpPost("create")]
    public async Task<ActionResult> CreateTrainer([FromForm] CreateTrainerDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await trainerMemberService.CreateTrainerAsync(dto);

            return Ok($"Trainer created successfully with username {dto.Username}");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Error = "An error occurred while creating trainer" });
        }
    }
    
    [HttpPut("update")]
    public async Task<ActionResult> UpdateTrainer(int id, [FromForm] UpdateTrainerDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await trainerMemberService.UpdateTrainerAsync(id, dto);

            return Ok($"Trainer updated successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Error = "An error occurred while updating trainer" });
        }
    }
    
    [HttpDelete("delete")]
    public async Task<ActionResult> DeleteTrainer(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await trainerMemberService.DeleteTrainerAsync(id);

            return Ok($"Trainer deleted successfully.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Error = "An error occurred while deleting trainer." });
        }
    }
    
    [HttpGet("{trainerId}/members")]
    public async Task<ActionResult<List<TrainerMemberDto>>> GetTrainerMembers(int trainerId)
    {
        var members = await trainerMemberService.GetTrainerMembersAsync(trainerId);
        return Ok(members);
    }
    
    [HttpPost("{trainerId}/members")]
    public async Task<ActionResult> AssignMemberToTrainer(int trainerId, [FromForm] AssignMemberToTrainerDto dto)
    {
        if (trainerId != dto.TrainerId)
            return BadRequest(new { Error = "Trainer ID mismatch" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var relationshipId = await trainerMemberService.AssignMemberToTrainerAsync(dto);
            return CreatedAtAction(nameof(GetTrainerMembers), new { trainerId }, 
                new { Id = relationshipId, Message = "Member assigned to trainer successfully" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Error = "An error occurred while assigning member to trainer" });
        }
    }
    
    [HttpGet("{memberId}/trainer")]
    public async Task<ActionResult<TrainerMemberDto>> GetMemberTrainer(int memberId)
    {
        var trainer = await trainerMemberService.GetMemberTrainerAsync(memberId);
        if (trainer == null)
            return Ok("Member is not assigned to any trainer");

        return Ok(trainer);
    }
    
    [HttpDelete("{trainerId}/members/{memberId}")]
    public async Task<ActionResult> RemoveMemberFromTrainer(int trainerId, int memberId)
    {
        var success = await trainerMemberService.RemoveMemberFromTrainerAsync(trainerId, memberId);
        if (!success)
            return NotFound(new { Error = "Training relationship not found" });

        return Ok(new { Message = "Member removed from trainer successfully" });
    }
}
