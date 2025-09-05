using System.ComponentModel.DataAnnotations;

namespace gym_management_api.DTO;

public class AssignMemberToTrainerDto
{
    [Required]
    public int TrainerId { get; set; }
        
    [Required]
    public int MemberId { get; set; }
        
    [StringLength(1000)]
    public string TrainingGoals { get; set; }
        
    [StringLength(1000)]
    public string Notes { get; set; }
        
    [Range(1, 7)]
    public int SessionsPerWeek { get; set; } = 1;
        
    [Range(0, 1000)]
    public decimal SessionRate { get; set; }
}