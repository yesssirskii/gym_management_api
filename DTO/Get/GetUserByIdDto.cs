using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class GetUserByIdDto
{
    public int Id { get; set; }
    public long Oib { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public GenderEnum Gender { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public SubscriptionTypeEnum SubscriptionType { get; set; }
    public string UserType { get; set; }
}