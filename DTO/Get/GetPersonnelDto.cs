using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class GetPersonnelDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int EmployeeId { get; set; }
    public DateTime HireDate { get; set; }
    public PersonnelRoleEnum Role { get; set; }
}