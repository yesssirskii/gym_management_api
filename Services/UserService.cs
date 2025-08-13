using gym_management_api.DTO.Create;
using gym_management_api.DTO.Get;
using gym_management_api.DTO.Update;
using gym_management_api.Models;
using gym_management_api.Enums;
using Microsoft.EntityFrameworkCore;

namespace gym_management_api.Services;
public class UserService(ApplicationDbContext dbContext, SubscriptionService subscriptionService)
{
    public async Task<List<GetUsersDto>> GetUserDataForTable()
    {
        return await dbContext.Users.Select(u => new GetUsersDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Oib = u.Oib,
            SubscriptionType = u.SubscriptionType,
            UserType = u.GetType().Name
        }).ToListAsync();
    }

    public async Task<int> CreateUser(CreateUserDto userDto)
    {
        if (await dbContext.Users.AnyAsync(u => u.Username == userDto.Username))
            throw new ArgumentException("Username already exists");
    
        if (await dbContext.Users.AnyAsync(u => u.Email == userDto.Email))
            throw new ArgumentException("Email already exists");

        User user = userDto.UserType switch
        {
            UserType.Member => new Member
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
                DateOfBirth = userDto.DateOfBirth,
                Gender = userDto.Gender,
                Address = userDto.Address,
                SubscriptionType = userDto.SubscriptionType,
                MembershipNumber = $"MEM{DateTime.UtcNow:yyyyMMdd}{new Random().Next(1000, 9999)}",
                EmergencyContactName = userDto.EmergencyContactName,
                EmergencyContactPhone = userDto.EmergencyContactPhone,
                MedicalNotes = userDto.MedicalNotes,
                FitnessGoals = userDto.FitnessGoals,
                Height = userDto.Height,
                Weight = userDto.Weight,
            },

            UserType.Trainer => new Trainer
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
                DateOfBirth = userDto.DateOfBirth,
                Gender = userDto.Gender,
                Address = userDto.Address,
                SubscriptionType = userDto.SubscriptionType,
                Specialization = userDto.Specialization,
                Certifications = userDto.Certifications,
                YearsOfExperience = userDto.YearsOfExperience ?? 0,
                HourlyRate = userDto.HourlyRate ?? 0,
                Bio = userDto.Bio
            },

            UserType.Personnel => new Personnel
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
                DateOfBirth = userDto.DateOfBirth,
                Gender = userDto.Gender,
                Address = userDto.Address,
                SubscriptionType = userDto.SubscriptionType,
                Role = userDto.Role ?? PersonnelRoleEnum.Receptionist,
                Salary = userDto.Salary ?? 0,
                EmployeeId = userDto.EmployeeId,
                JobDescription = userDto.JobDescription
            },
            _ => throw new ArgumentException("Invalid user type")
        };
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        
        if (userDto.SubscriptionType != SubscriptionTypeEnum.None)
        {
            var subscription = new CreateSubscriptionDto()
            {
                MemberId = user.Id,
                Type = user.SubscriptionType,
                
                StartDate = DateTime.UtcNow, 
                EndDate = DateTime.UtcNow.AddMonths(user.SubscriptionType == SubscriptionTypeEnum.Monthly ? 1 : 12),
                
                Price = user.SubscriptionType == SubscriptionTypeEnum.Monthly ? 10 : 30,
                Status = SubscriptionStatusEnum.Active,
                PaymentMethod = PaymentMethodEnum.Cash,
                AutoRenewal = true,
                
                IsCancelled = false,
                CancelledAt = null
            };
            
            await subscriptionService.CreateSubscriptionAsync(subscription);
        }
        
        return user.Id;
    }
    
    public async Task<bool> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null) return false;

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
        user.Gender = dto.Gender;
        user.Address = dto.Address ?? user.Address;
        user.Email = dto.Email ?? user.Email;
        user.Oib = dto.Oib;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        
        return true;
    }
    
    public async Task DeleteUser(int id)
    {
        User? user = await dbContext.Users.FindAsync(id);
        
        if (user != null)
        {
            user.IsDeleted = true;
            user.IsActive = false;
            user.DeletedAt = DateTime.UtcNow;

            // checking if the user has a subscription
            if (user.SubscriptionType != SubscriptionTypeEnum.None)
            {
                Subscription? subscription = await subscriptionService.GetSubscriptionById(user.Id);
                
                subscription.Status = SubscriptionStatusEnum.Cancelled;
                subscription.IsCancelled = true;
                subscription.CancelledAt = DateTime.UtcNow;
            }
            
            await dbContext.SaveChangesAsync();
        }
        
        await dbContext.SaveChangesAsync();
    }
}