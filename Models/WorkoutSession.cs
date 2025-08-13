using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gym_management_api.Enums;

namespace gym_management_api.Models;

public class WorkoutSession
{
    public int Id { get; set; }
        
    public int TrainerId { get; set; }
    public virtual Trainer Trainer { get; set; }
        
    public int MemberId { get; set; }
    public virtual Member Member { get; set; }
        
    public DateTime ScheduledDate { get; set; }
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }
        
    public SessionStatusEnum Status { get; set; } = SessionStatusEnum.Scheduled;
        
    [StringLength(500)]
    public string WorkoutPlan { get; set; }
        
    [StringLength(500)]
    public string Notes { get; set; }
        
    [Column(TypeName = "decimal(3,2)")]
    public decimal? MemberRating { get; set; }
        
    [StringLength(500)]
    public string MemberFeedback { get; set; }
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}