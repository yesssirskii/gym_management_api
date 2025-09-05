using System.ComponentModel.DataAnnotations;
using gym_management_api.Enums;

namespace gym_management_api.DTO.Update;

public class UpdateUserDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }
    public long Oib { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
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
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public SubscriptionTypeEnum SubscriptionType { get; set; }
    
    [Required]
    public UpdateUserType UserType { get; set; }
    
    // Member-specific fields
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? MedicalNotes { get; set; }
    public string? FitnessGoals { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    
    // Trainer-specific fields
    public TrainerSpecializationEnum Specialization { get; set; }
    public string? Certifications { get; set; }
    public int? YearsOfExperience { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Bio { get; set; }
    
    // Personnel-specific fields
    public PersonnelRoleEnum? Role { get; set; }
    public decimal? Salary { get; set; }
    public string? JobDescription { get; set; }
}

public enum UpdateUserType
{
    Member = 1,
    Trainer = 2,
    Personnel = 3,
    Owner = 4,
    Manager = 5
}