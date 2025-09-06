using gym_management_api.DTO.Create;
using gym_management_api.DTO.Get;
using gym_management_api.DTO.Update;

namespace gym_management_api.Interfaces;

public interface IUserInterface
{
    Task<List<GetUsersDto>> GetUserDataForTable();
    Task<GetUserByIdDto?> GetUserById(int id);
    Task<List<GetUsersDto>> GetMembers();
    Task<List<GetPersonnelDto>> GetPersonnel();
    Task<List<GetUsersDto>> GetTrainers();
    Task<int> CreateUserAsync(CreateUserDto userDto);
    Task<bool> UpdateUserAsync(CreateUserDto dto);
    Task DeleteUser(int id);
}