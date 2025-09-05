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
            CreatedAt = u.CreatedAt,
            IsActive = u.IsActive,
            Oib = u.Oib,
            SubscriptionType = u.SubscriptionType,
            UserType = u.GetType().Name
        })
        .ToListAsync();
    }

    public async Task<GetUserByIdDto?> GetUserById(int id)
    {
        var user = await dbContext.Users.FindAsync(id);
        
        if (user == null)
            return null;

        return user switch
        {
            Member member => new GetMemberByIdDto
            {
                Id = member.Id,
                Oib = member.Oib,
                Username = member.Username,
                Email = member.Email,
                FirstName = member.FirstName,
                LastName = member.LastName,
                PhoneNumber = member.PhoneNumber,
                DateOfBirth = member.DateOfBirth,
                Gender = member.Gender,
                Address = member.Address,
                CreatedAt = member.CreatedAt,
                UpdatedAt = member.UpdatedAt,
                IsActive = member.IsActive,
                ProfileImageUrl = member.ProfileImageUrl,
                LastLoginAt = member.LastLoginAt,
                SubscriptionType = member.SubscriptionType,
                UserType = nameof(Member),
                
                MembershipNumber = member.MembershipNumber,
                JoinDate = member.JoinDate,
                EmergencyContactName = member.EmergencyContactName,
                EmergencyContactPhone = member.EmergencyContactPhone,
                MedicalNotes = member.MedicalNotes,
                FitnessGoals = member.FitnessGoals,
                Height = member.Height,
                Weight = member.Weight
            },
            
            Trainer trainer => new GetTrainerByIdDto
            {
                Id = trainer.Id,
                Oib = trainer.Oib,
                Username = trainer.Username,
                Email = trainer.Email,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                PhoneNumber = trainer.PhoneNumber,
                DateOfBirth = trainer.DateOfBirth,
                Gender = trainer.Gender,
                Address = trainer.Address,
                CreatedAt = trainer.CreatedAt,
                UpdatedAt = trainer.UpdatedAt,
                IsActive = trainer.IsActive,
                ProfileImageUrl = trainer.ProfileImageUrl,
                LastLoginAt = trainer.LastLoginAt,
                SubscriptionType = trainer.SubscriptionType,
                UserType = nameof(Trainer),
                
                Specialization = trainer.Specialization,
                Certifications = trainer.Certifications,
                YearsOfExperience = trainer.YearsOfExperience,
                HourlyRate = trainer.HourlyRate,
                Rating = trainer.Rating,
                Bio = trainer.Bio,
                IsAvailable = trainer.IsAvailable,
                PersonnelId = trainer.PersonnelId
            },
            
            Personnel personnel => new GetPersonnelByIdDto
            {
                Id = personnel.Id,
                Oib = personnel.Oib,
                Username = personnel.Username,
                Email = personnel.Email,
                FirstName = personnel.FirstName,
                LastName = personnel.LastName,
                PhoneNumber = personnel.PhoneNumber,
                DateOfBirth = personnel.DateOfBirth,
                Gender = personnel.Gender,
                Address = personnel.Address,
                CreatedAt = personnel.CreatedAt,
                UpdatedAt = personnel.UpdatedAt,
                IsActive = personnel.IsActive,
                ProfileImageUrl = personnel.ProfileImageUrl,
                LastLoginAt = personnel.LastLoginAt,
                SubscriptionType = personnel.SubscriptionType,
                UserType = nameof(Personnel),
                
                EmployeeId = personnel.EmployeeId,
                Role = personnel.Role,
                Salary = personnel.Salary,
                HireDate = personnel.HireDate,
                JobDescription = personnel.JobDescription
            },
            
            _ => new GetUserByIdDto
            {
                Id = user.Id,
                Oib = user.Oib,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsActive = user.IsActive,
                ProfileImageUrl = user.ProfileImageUrl,
                LastLoginAt = user.LastLoginAt,
                SubscriptionType = user.SubscriptionType,
                UserType = user.GetType().Name
            }
        };
    }
    
    public async Task<List<GetUsersDto>> GetMembers()
    {
        var members = await dbContext.Users
            .OfType<Member>()
            .Where(u => u.IsDeleted == false)
            .Select(u => new GetUsersDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                CreatedAt = u.CreatedAt,
                IsActive = u.IsActive,
                Oib = u.Oib,
                SubscriptionType = u.SubscriptionType,
                UserType = "Member"
            })
            .ToListAsync();

        return members;
    }
    
    public async Task<List<GetUsersDto>> GetPersonnel()
    {
        var personnel = await dbContext.Users
            .OfType<Personnel>()
            .Where(u => u.IsDeleted == false)
            .Select(u => new GetUsersDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                CreatedAt = u.CreatedAt,
                IsActive = u.IsActive,
                Oib = u.Oib,
                SubscriptionType = u.SubscriptionType,
                UserType = "Personnel"
            })
            .ToListAsync();

        return personnel;
    }
    
    public async Task<List<GetUsersDto>> GetTrainers()
    {
        var trainers = await dbContext.Users
            .OfType<Trainer>()
            .Where(u => u.IsDeleted == false)
            .Select(u => new GetUsersDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                CreatedAt = u.CreatedAt,
                IsActive = u.IsActive,
                Oib = u.Oib,
                SubscriptionType = u.SubscriptionType,
                UserType = "Trainer"
            })
            .ToListAsync();

        return trainers;
    }

    public async Task<int> CreateUser(CreateUserDto userDto)
    {
        if (await dbContext.Users.AnyAsync(u => u.Username == userDto.Username))
            throw new ArgumentException("Username already exists");
    
        if (await dbContext.Users.AnyAsync(u => u.Email == userDto.Email))
            throw new ArgumentException("Email already exists");
        
        DateTime dateOfBirth = DateTime.SpecifyKind(userDto.DateOfBirth, DateTimeKind.Utc);

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
                DateOfBirth = dateOfBirth,
                Gender = userDto.Gender,
                Address = userDto.Address ?? "",
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
                DateOfBirth = dateOfBirth,
                Gender = userDto.Gender,
                Address = userDto.Address ?? "",
                SubscriptionType = userDto.SubscriptionType,
                Specialization = userDto.Specialization,
                Certifications = userDto.Certifications ?? "",
                YearsOfExperience = userDto.YearsOfExperience ?? 0,
                HourlyRate = userDto.HourlyRate ?? 0,
                Bio = userDto.Bio ?? "",
            },

            UserType.Personnel => new Personnel
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber,
                DateOfBirth = dateOfBirth,
                Gender = userDto.Gender,
                Address = userDto.Address ?? "",
                SubscriptionType = userDto.SubscriptionType,
                Role = userDto.Role ?? PersonnelRoleEnum.Receptionist,
                Salary = userDto.Salary ?? 0,
                JobDescription = userDto.JobDescription
            },
            _ => throw new ArgumentException("Invalid user type")
        };
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        
        if (userDto.SubscriptionType != 0)
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
            if (user.SubscriptionType != 0)
            {
                Subscription? subscription = await subscriptionService.GetSubscriptionByMemberId(user.Id);
                
                subscription.Status = SubscriptionStatusEnum.Cancelled;
                subscription.IsCancelled = true;
                subscription.CancelledAt = DateTime.UtcNow;
            }
            
            await dbContext.SaveChangesAsync();
        }
        
        await dbContext.SaveChangesAsync();
    }
}