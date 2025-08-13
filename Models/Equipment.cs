using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gym_management_api.Enums;

namespace gym_management_api.Models;

public class Equipment
{
    public int Id { get; set; }
        
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
        
    [StringLength(500)]
    public string Description { get; set; }
        
    [StringLength(50)]
    public string Brand { get; set; }
        
    [StringLength(50)]
    public string Model { get; set; }
        
    public DateTime PurchaseDate { get; set; }
        
    [Column(TypeName = "decimal(10,2)")]
    public decimal PurchasePrice { get; set; }
        
    public EquipmentStatusEnum Status { get; set; } = EquipmentStatusEnum.Available;
        
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
        
    [StringLength(500)]
    public string MaintenanceNotes { get; set; }
        
    [StringLength(255)]
    public string Location { get; set; }
}