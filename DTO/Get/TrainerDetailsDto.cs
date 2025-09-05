using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class TrainerDetailsDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public TrainerSpecializationEnum Specialization { get; set; }
    public int YearsOfExperience { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal Rating { get; set; }
    public bool IsAvailable { get; set; }
    public int? ActiveMembersCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public GenderEnum Gender { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Certifications { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUrl { get; set; }
    public DateTime? LastLoginAt { get; set; }
        
    // Members assigned to this trainer
    public List<TrainerMemberDto>? AssignedMembers { get; set; } = new List<TrainerMemberDto>();
}