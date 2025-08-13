using gym_management_api.DTO;
using gym_management_api.DTO.Create;
using gym_management_api.DTO.Get;
using gym_management_api.DTO.Update;

namespace gym_management_api.Interfaces;

public interface ITrainerMemberService
{
    Task<List<GetTrainersDto>> GetAllTrainersAsync();
    Task<TrainerDetailsDto> GetTrainerByIdAsync(int id);
    Task<int> CreateTrainerAsync(CreateTrainerDto dto);
    Task<bool> UpdateTrainerAsync(int id, UpdateTrainerDto dto);
    Task<bool> DeleteTrainerAsync(int id);
    Task <int> AssignMemberToTrainerAsync(AssignMemberToTrainerDto dto);
    Task<bool> RemoveMemberFromTrainerAsync(int trainerId, int memberId);
    Task<TrainerMemberDto> GetMemberTrainerAsync(int memberId);
    Task<List<TrainerMemberDto>> GetTrainerMembersAsync(int trainerId);
}