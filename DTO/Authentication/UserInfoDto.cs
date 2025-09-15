namespace gym_management_api.DTO.Authentication;

public class UserInfoDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserType { get; set; }
    public string Role { get; set; } // For personnel
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}