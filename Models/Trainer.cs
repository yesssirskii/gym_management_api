using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gym_management_api.Enums;

namespace gym_management_api.Models;

public class Trainer : User
{
    [StringLength(100)]
    public TrainerSpecializationEnum Specialization { get; set; }
        
    [StringLength(1000)]
    public string Certifications { get; set; }
        
    public int YearsOfExperience { get; set; }
        
    [Column(TypeName = "decimal(8,2)")]
    public decimal HourlyRate { get; set; }
        
    [Column(TypeName = "decimal(3,2)")]
    public decimal Rating { get; set; } = 0;
        
    [StringLength(1000)]
    public string Bio { get; set; }
        
    public bool IsAvailable { get; set; } = true;
        
    public int? PersonnelId { get; set; }
    public virtual Personnel Personnel { get; set; }
        
    public virtual ICollection<TrainerMember> TrainerMembers { get; set; } = new List<TrainerMember>();
}