using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class GetPersonnelByIdDto : GetUserByIdDto
{
    public PersonnelRoleEnum Role { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public string EmployeeId { get; set; }
    public string? JobDescription { get; set; }
}