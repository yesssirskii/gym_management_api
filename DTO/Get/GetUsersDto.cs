using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class GetUsersDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public long Oib { get; set; }
    public SubscriptionTypeEnum? SubscriptionType { get; set; }
    public string UserType { get; set; }

}