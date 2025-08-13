using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class GetTrainersDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public  string Email { get; set; }
    public string PhoneNumber { get; set; }
    public TrainerSpecializationEnum Specialization { get; set; }
    public  int YearsOfExperience { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal Rating { get; set; }
    public bool IsAvailable { get; set; }
    public int ActiveMembersCount { get; set; }
    public DateTime CreatedAt { get; set; }
}