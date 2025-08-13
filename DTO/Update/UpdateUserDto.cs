using System.ComponentModel.DataAnnotations;
using gym_management_api.Enums;

namespace gym_management_api.DTO.Update;

public class UpdateUserDto
{
    [StringLength(100)]
    public required string FirstName { get; set; }
        
    [StringLength(100)]
    public string LastName { get; set; }
        
    [Phone]
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required long Oib { get; set; }
    public GenderEnum Gender { get; set; }
    public required string Address { get; set; }
    public bool IsActive { get; set; }
}