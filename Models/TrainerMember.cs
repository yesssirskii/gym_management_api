using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gym_management_api.Enums;

namespace gym_management_api.Models;

public class TrainerMember
{
    public int Id { get; set; }
        
    public int TrainerId { get; set; }
    public virtual Trainer? Trainer { get; set; }
        
    public int MemberId { get; set; }
    public virtual Member Member { get; set; }
        
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
        
    public TrainingStatusEnum Status { get; set; } = TrainingStatusEnum.Active;
        
    [StringLength(1000)]
    public string? TrainingGoals { get; set; }
        
    [StringLength(1000)]
    public string? Notes { get; set; }
        
    public required int SessionsPerWeek { get; set; }
        
    [Column(TypeName = "decimal(8,2)")]
    public required decimal SessionRate { get; set; }
}