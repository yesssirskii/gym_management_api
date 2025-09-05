using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gym_management_api.Enums;

namespace gym_management_api.Models;

public class Personnel : User
{
    [Required]
    public int EmployeeId { get; set; }
    
    [Required]
    public PersonnelRoleEnum Role { get; set; }
        
    [Column(TypeName = "decimal(10,2)")]
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
        
    [StringLength(500)]
    public string? JobDescription { get; set; }
        
    // Navigation properties
    public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
}