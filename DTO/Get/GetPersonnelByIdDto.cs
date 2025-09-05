using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class GetPersonnelByIdDto : GetUserByIdDto
{
    public int EmployeeId { get; set; }
    public PersonnelRoleEnum Role { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public string? JobDescription { get; set; }
}