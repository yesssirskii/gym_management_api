using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class TrainerMemberDto
{
    public int Id { get; set; }
    public int TrainerId { get; set; }
    public int MemberId { get; set; }
        
    // Member info
    public string MemberUsername { get; set; }
    public string MemberFirstName { get; set; }
    public string MemberLastName { get; set; }
    public string MemberFullName => $"{MemberFirstName} {MemberLastName}";
    public string MemberEmail { get; set; }
    public string MemberPhone { get; set; }
    public string MembershipNumber { get; set; }
    public bool HasActiveSubscription { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
        
    // Trainer info (when viewing from member perspective)
    public string TrainerUsername { get; set; }
    public string TrainerFirstName { get; set; }
    public string TrainerLastName { get; set; }
    public string TrainerFullName => $"{TrainerFirstName} {TrainerLastName}";
    public TrainerSpecializationEnum TrainerSpecialization { get; set; }
    public decimal TrainerRating { get; set; }
        
    // Training relationship info
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TrainingStatusEnum Status { get; set; }
    public string TrainingGoals { get; set; }
    public string Notes { get; set; }
    public int SessionsPerWeek { get; set; }
    public decimal SessionRate { get; set; }
        
    // Calculated fields
    public int TrainingDurationDays => (EndDate ?? DateTime.UtcNow).Subtract(StartDate).Days;
    public bool IsActive => Status == TrainingStatusEnum.Active && (EndDate == null || EndDate > DateTime.UtcNow);
}