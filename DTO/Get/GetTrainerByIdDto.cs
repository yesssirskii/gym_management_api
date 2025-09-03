using gym_management_api.Enums;

namespace gym_management_api.DTO.Get;

public class GetTrainerByIdDto : GetUserByIdDto
{
    public TrainerSpecializationEnum Specialization { get; set; }
    public string Certifications { get; set; }
    public int YearsOfExperience { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal Rating { get; set; }
    public string Bio { get; set; }
    public bool IsAvailable { get; set; }
    public int? PersonnelId { get; set; }
}