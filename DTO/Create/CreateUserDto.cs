using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using gym_management_api.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace gym_management_api.DTO.Create;

public class CreateUserDto
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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public GenderEnum Gender { get; set; }
    public string? Address { get; set; }
    
    public SubscriptionTypeEnum SubscriptionType { get; set; }
    
    [Required]
    public UserType UserType { get; set; }
    
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

public enum UserType
{
    Member = 1,
    Trainer = 2,
    Personnel = 3,
    Owner = 4,
    Manager = 5
}