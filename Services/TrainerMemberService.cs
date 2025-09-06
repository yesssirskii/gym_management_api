using System.Runtime.InteropServices.JavaScript;
using gym_management_api.DTO;
using gym_management_api.DTO.Create;
using gym_management_api.DTO.Get;
using gym_management_api.DTO.Update;
using gym_management_api.Enums;
using gym_management_api.Interfaces;
using gym_management_api.Models;
using Microsoft.EntityFrameworkCore;

namespace gym_management_api.Services;

public class TrainerMemberService(ApplicationDbContext dbContext) : ITrainerMemberService
{
    public async Task<List<GetTrainersDto>> GetAllTrainersAsync()
    {
        return await dbContext.Trainers
            .Include(t => t.TrainerMembers.Where(tm => tm.Status == TrainingStatusEnum.Active))
            .Select(t => new GetTrainersDto()
            {
                Id = t.Id,
                Username = t.Username,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Specialization = t.Specialization,
                YearsOfExperience = t.YearsOfExperience,
                HourlyRate = t.HourlyRate,
                Rating = t.Rating,
                IsAvailable = t.IsAvailable,
                ActiveMembersCount = t.TrainerMembers.Count(tm => tm.Status == TrainingStatusEnum.Active),
                CreatedAt = t.CreatedAt
            })
            .OrderBy(t => t.LastName)
            .ToListAsync();
    }
    
    public async Task<TrainerDetailsDto> GetTrainerByIdAsync(int id)
        {
            try
            {
                var trainer = await dbContext.Trainers
                    .AsSplitQuery()
                    .Include(t => t.TrainerMembers)
                    .ThenInclude(tm => tm.Member)
                    .ThenInclude(m => m.Subscriptions.Where(s => s.Status == SubscriptionStatusEnum.Active))
                    .Include(t => t.TrainerMembers.Where(tm => tm.Status == TrainingStatusEnum.Active))
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (trainer == null) return null;

                var dto = new TrainerDetailsDto
                {
                    Id = trainer.Id,
                    Username = trainer.Username,
                    FirstName = trainer.FirstName,
                    LastName = trainer.LastName,
                    Email = trainer.Email,
                    Oib = trainer.Oib,
                    PhoneNumber = trainer.PhoneNumber,
                    Gender = trainer.Gender,
                    Address = trainer.Address,
                    DateOfBirth = trainer.DateOfBirth,
                    
                    Specialization = trainer.Specialization,
                    Certifications = trainer.Certifications,
                    YearsOfExperience = trainer.YearsOfExperience,
                    HourlyRate = trainer.HourlyRate,
                    Rating = trainer.Rating,
                    Bio = trainer.Bio,
                    ProfileImageUrl = trainer.ProfileImageUrl,
                    IsAvailable = trainer.IsAvailable,
                    CreatedAt = trainer.CreatedAt,
                    LastLoginAt = trainer.LastLoginAt,
                    ActiveMembersCount = trainer.TrainerMembers.Count(tm => tm.Status == TrainingStatusEnum.Active),
                };

                // Map assigned members
                dto.AssignedMembers = trainer.TrainerMembers.Select(tm => new TrainerMemberDto
                {
                    Id = tm.Id,
                    TrainerId = tm.TrainerId,
                    MemberId = tm.MemberId,
                    MemberUsername = tm.Member.Username,
                    MemberFirstName = tm.Member.FirstName,
                    MemberLastName = tm.Member.LastName,
                    MemberEmail = tm.Member.Email,
                    MemberPhone = tm.Member.PhoneNumber,
                    MembershipNumber = tm.Member.MembershipNumber,
                    HasActiveSubscription = tm.Member.Subscriptions.Any(s =>
                        s.Status == SubscriptionStatusEnum.Active && s.EndDate > DateTime.UtcNow),
                    SubscriptionEndDate = tm.Member.Subscriptions
                        .FirstOrDefault(s => s.Status == SubscriptionStatusEnum.Active)?.EndDate,
                    StartDate = tm.StartDate,
                    EndDate = tm.EndDate,
                    Status = tm.Status,
                    TrainingGoals = tm.TrainingGoals,
                    Notes = tm.Notes,
                    SessionsPerWeek = tm.SessionsPerWeek,
                    SessionRate = tm.SessionRate
                }).ToList();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error while fetching trainer details: {ex.Message}") ;
            }
        }
    
    public async Task<int> CreateTrainerAsync(CreateTrainerDto dto)
    {
        if (await dbContext.Users.AnyAsync(u => u.Username == dto.Username))
            throw new ArgumentException("Username already exists");
            
        if (await dbContext.Users.AnyAsync(u => u.Email == dto.Email))
            throw new ArgumentException("Email already exists");

        var trainer = new Trainer()
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            DateOfBirth = dto.DateOfBirth.ToUniversalTime(),
            Gender = dto.Gender,
            Address = dto.Address,
            Specialization = dto.Specialization,
            Certifications = dto.Certifications,
            YearsOfExperience = dto.YearsOfExperience,
            HourlyRate = dto.HourlyRate,
            IsAvailable = dto.IsAvailable,
            Bio = dto.Bio,
            //PersonnelId = dto.PersonnelId,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Trainers.Add(trainer);
        await dbContext.SaveChangesAsync();
            
        return trainer.Id;
    }
    
    public async Task<bool> UpdateTrainerAsync(int id, UpdateTrainerDto dto)
        {
            var trainer = await dbContext.Trainers.FindAsync(id);
            if (trainer == null) return false;

            // Update fields only if provided
            if (!string.IsNullOrEmpty(dto.FirstName)) trainer.FirstName = dto.FirstName;
            if (!string.IsNullOrEmpty(dto.LastName)) trainer.LastName = dto.LastName;
            if (!string.IsNullOrEmpty(dto.PhoneNumber)) trainer.PhoneNumber = dto.PhoneNumber;
            trainer.Gender = dto.Gender;
            if (!string.IsNullOrEmpty(dto.Address)) trainer.Address = dto.Address;
            trainer.Specialization = dto.Specialization;
            if (!string.IsNullOrEmpty(dto.Certifications)) trainer.Certifications = dto.Certifications;
            trainer.YearsOfExperience = dto.YearsOfExperience;
            trainer.HourlyRate = dto.HourlyRate;
            if (!string.IsNullOrEmpty(dto.Bio)) trainer.Bio = dto.Bio;
            if (dto.IsAvailable.HasValue) trainer.IsAvailable = dto.IsAvailable.Value;
            if (dto.PersonnelId.HasValue) trainer.PersonnelId = dto.PersonnelId.Value;
            
            trainer.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteTrainerAsync(int id)
        {
            var trainer = await dbContext.Trainers.FindAsync(id);
            if (trainer == null) return false;

            // Soft delete
            trainer.IsActive = false;
            trainer.UpdatedAt = DateTime.UtcNow;

            // End all active training relationships
            var activeTrainingRelationships = await dbContext.TrainerMembers
                .Where(tm => tm.TrainerId == id && tm.Status == TrainingStatusEnum.Active)
                .ToListAsync();

            foreach (var relationship in activeTrainingRelationships)
            {
                relationship.Status = TrainingStatusEnum.Cancelled;
                relationship.EndDate = DateTime.UtcNow;
            }

            await dbContext.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<int> AssignMemberToTrainerAsync(AssignMemberToTrainerDto dto)
        {
            // Validate trainer exists and is available
            var trainer = await dbContext.Trainers.FindAsync(dto.TrainerId);
            if (trainer == null || !trainer.IsAvailable)
                throw new ArgumentException("Trainer not found or unavailable");

            if (!trainer.IsAvailable)
                throw new ArgumentException("Trainer is not available for new members");

            // Validate member exists
            var member = await dbContext.Members.FindAsync(dto.MemberId);
            if (member == null || !member.IsActive)
                throw new ArgumentException("Member not found or inactive");

            // Check if relationship already exists
            var existingRelationship = await dbContext.TrainerMembers
                .FirstOrDefaultAsync(tm => tm.TrainerId == dto.TrainerId && 
                                           tm.MemberId == dto.MemberId && 
                                           tm.Status == TrainingStatusEnum.Active);

            if (existingRelationship != null)
                throw new ArgumentException("Member is already assigned to this trainer");

            // Create new trainer-member relationship
            var trainerMember = new TrainerMember
            {
                TrainerId = dto.TrainerId,
                MemberId = dto.MemberId,
                TrainingGoals = dto.TrainingGoals,
                Notes = dto.Notes,
                SessionsPerWeek = dto.SessionsPerWeek,
                SessionRate = dto.SessionRate,
                StartDate = DateTime.UtcNow,
                Status = TrainingStatusEnum.Active
            };

            dbContext.TrainerMembers.Add(trainerMember);
            await dbContext.SaveChangesAsync();

            return trainerMember.Id;
        }
        
        public async Task<bool> RemoveMemberFromTrainerAsync(int trainerId, int memberId)
        {
            var relationship = await dbContext.TrainerMembers
                .FirstOrDefaultAsync(tm => tm.TrainerId == trainerId && 
                                           tm.MemberId == memberId && 
                                           tm.Status == TrainingStatusEnum.Active);

            if (relationship == null) return false;

            relationship.Status = TrainingStatusEnum.Completed;
            relationship.EndDate = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return true;
        }
        
        public async Task<TrainerMemberDto> GetMemberTrainerAsync(int memberId)
        {
            var memberTrainer = await dbContext.TrainerMembers
                .Include(tm => tm.Trainer)
                .Include(tm => tm.Member)
                .Where(tm => tm.MemberId == memberId)
                .Select(tm => new TrainerMemberDto
                {
                    Id = tm.Id,
                    TrainerId = tm.TrainerId,
                    MemberId = tm.MemberId,
                    TrainerUsername = tm.Trainer.Username,
                    TrainerFirstName = tm.Trainer.FirstName,
                    TrainerLastName = tm.Trainer.LastName,
                    TrainerSpecialization = tm.Trainer.Specialization,
                    TrainerRating = tm.Trainer.Rating,
                    MemberUsername = tm.Member.Username,
                    MemberFirstName = tm.Member.FirstName,
                    MemberLastName = tm.Member.LastName,
                    StartDate = tm.StartDate,
                    EndDate = tm.EndDate,
                    Status = tm.Status,
                    TrainingGoals = tm.TrainingGoals,
                    Notes = tm.Notes,
                    SessionsPerWeek = tm.SessionsPerWeek,
                    SessionRate = tm.SessionRate
                })
                .FirstOrDefaultAsync();
            
            return memberTrainer;
        }
        
        public async Task<List<TrainerMemberDto>> GetTrainerMembersAsync(int trainerId)
        {
            return await dbContext.TrainerMembers
                .Include(tm => tm.Member)
                .ThenInclude(m => m.Subscriptions.Where(s => s.Status == SubscriptionStatusEnum.Active))
                .Where(tm => tm.TrainerId == trainerId)
                .Where(tm => tm.Status == TrainingStatusEnum.Active)
                .Select(tm => new TrainerMemberDto
                {
                    Id = tm.Id,
                    TrainerId = tm.TrainerId,
                    MemberId = tm.MemberId,
                    MemberUsername = tm.Member.Username,
                    MemberFirstName = tm.Member.FirstName,
                    MemberLastName = tm.Member.LastName,
                    MemberEmail = tm.Member.Email,
                    MemberPhone = tm.Member.PhoneNumber,
                    MembershipNumber = tm.Member.MembershipNumber,
                    HasActiveSubscription = tm.Member.Subscriptions.Any(s => s.Status == SubscriptionStatusEnum.Active && s.EndDate > DateTime.UtcNow),
                    SubscriptionEndDate = tm.Member.Subscriptions.FirstOrDefault(s => s.Status == SubscriptionStatusEnum.Active).EndDate,
                    StartDate = tm.StartDate,
                    EndDate = tm.EndDate,
                    Status = tm.Status,
                    TrainingGoals = tm.TrainingGoals,
                    Notes = tm.Notes,
                    SessionsPerWeek = tm.SessionsPerWeek,
                    SessionRate = tm.SessionRate
                })
                .OrderByDescending(tm => tm.StartDate)
                .ToListAsync();
        }
}