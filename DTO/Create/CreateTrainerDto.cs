using System.ComponentModel.DataAnnotations;
using gym_management_api.Enums;


namespace gym_management_api.DTO.Create;

public class CreateTrainerDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }
        
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
        
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
        
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
        
    [Phone]
    public string PhoneNumber { get; set; }
        
    [Required]
    public DateTime DateOfBirth { get; set; }
        
    public GenderEnum Gender { get; set; }
    
    [Required]
    public string Address { get; set; }
        
    // Trainer-specific fields
    [Required]
    public TrainerSpecializationEnum Specialization { get; set; }
        
    [StringLength(500)]
    public string Certifications { get; set; }
        
    [Range(0, 50)]
    public int YearsOfExperience { get; set; } = 0;
        
    [Required]
    [Range(0, 1000)]
    public decimal HourlyRate { get; set; }
    public bool IsAvailable { get; set; }
        
    [StringLength(1000)]
    public string Bio { get; set; }
        
    // If trainer is also personnel
    //public int? PersonnelId { get; set; }
}