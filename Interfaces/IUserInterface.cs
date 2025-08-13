using gym_management_api.DTO.Create;
using gym_management_api.DTO.Get;
using gym_management_api.DTO.Update;

namespace gym_management_api.Interfaces;

public interface IUserInterface
{
    Task<List<GetUsersDto>> GetUserDataForTable();
    Task<int> CreateUser(CreateUserDto userDto);
    Task<bool> UpdateUserAsync(UpdateUserDto dto);
    Task DeleteUser(int id);
}