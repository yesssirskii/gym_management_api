using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gym_management_api.Models;

public class Member : User
{
    [StringLength(50)]
    public string? MembershipNumber { get; set; }
        
    public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        
    [StringLength(255)]
    public string? EmergencyContactName { get; set; }
        
    [StringLength(15)]
    public string? EmergencyContactPhone { get; set; }
        
    [StringLength(500)]
    public string? MedicalNotes { get; set; }
        
    [StringLength(500)]
    public string? FitnessGoals { get; set; }
        
    [Column(TypeName = "decimal(5,2)")]
    public decimal? Height { get; set; } // in cm
        
    [Column(TypeName = "decimal(5,2)")]
    public decimal? Weight { get; set; } // in kg
        
    // Navigation properties
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public virtual ICollection<TrainerMember> TrainerMembers { get; set; } = new List<TrainerMember>();
    public virtual ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
}