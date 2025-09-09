using gym_management_api.DTO;
using gym_management_api.Models;
using Microsoft.EntityFrameworkCore;

namespace gym_management_api;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Personnel> Personnel { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<TrainerMember> TrainerMembers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .ToTable("Users")
            .HasDiscriminator<string>("UserType")
            .HasValue<Member>("Member")
            .HasValue<Trainer>("Trainer")
            .HasValue<Personnel>("Personnel");

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasIndex(e => e.MembershipNumber).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();

            entity.HasOne(t => t.Personnel)
                .WithMany(p => p.Trainers)
                .HasForeignKey(t => t.PersonnelId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Personnel>(entity =>
        {
            entity.HasIndex(e => e.EmployeeId).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();

            entity.Property(e => e.EmployeeId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasOne(s => s.Member)
                .WithMany(m => m.Subscriptions)
                .HasForeignKey(s => s.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.MemberId, e.StartDate });
        });

        modelBuilder.Entity<TrainerMember>(entity =>
        {
            entity.HasOne(tm => tm.Trainer)
                .WithMany(t => t.TrainerMembers)
                .HasForeignKey(tm => tm.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tm => tm.Member)
                .WithMany(m => m.TrainerMembers)
                .HasForeignKey(tm => tm.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.TrainerId, e.MemberId });
        });
        
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(rt => rt.Id);
            
            entity.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(rt => rt.Token).IsUnique();
            entity.HasIndex(rt => new { rt.UserId, rt.IsRevoked });
            entity.HasIndex(rt => rt.ExpiresAt);

            entity.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(prt => prt.Id);
            
            entity.Property(prt => prt.Token)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(prt => prt.Token).IsUnique();
            entity.HasIndex(prt => new { prt.UserId, prt.IsUsed });
            entity.HasIndex(prt => prt.ExpiresAt);

            entity.HasOne(prt => prt.User)
                .WithMany()
                .HasForeignKey(prt => prt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}