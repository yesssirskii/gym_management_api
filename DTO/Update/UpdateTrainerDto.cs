using System.ComponentModel.DataAnnotations;
using gym_management_api.Enums;

namespace gym_management_api.DTO.Update;

public class UpdateTrainerDto
{
    [StringLength(100)]
    public required string FirstName { get; set; }
        
    [StringLength(100)]
    public required string LastName { get; set; }
        
    [Phone]
    public required string PhoneNumber { get; set; }
    public GenderEnum Gender { get; set; }
    public required string Address { get; set; }
    public TrainerSpecializationEnum Specialization { get; set; }
        
    [StringLength(500)]
    public string? Certifications { get; set; }
        
    [Range(0, 50)]
    public required int YearsOfExperience { get; set; }
        
    [Range(0, 1000)]
    public required decimal HourlyRate { get; set; }
        
    [StringLength(1000)]
    public string? Bio { get; set; }
        
    public bool? IsAvailable { get; set; }
        
    public int? PersonnelId { get; set; }
}