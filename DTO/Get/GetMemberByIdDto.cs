namespace gym_management_api.DTO.Get;

public class GetMemberByIdDto : GetUserByIdDto
{
    public string? MembershipNumber { get; set; }
    public DateTime StartDate { get; set; }
    public string? PaymentMethod { get; set; }
    public bool AutoRenewal { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? MedicalNotes { get; set; }
    public string? FitnessGoals { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    
    public GetSubscriptionByIdDto? Subscription { get; set; }
}