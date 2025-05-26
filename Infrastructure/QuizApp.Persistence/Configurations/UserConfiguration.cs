using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;
using QuizApp.Domain.Enums;

namespace QuizApp.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Username).HasColumnName("Username").IsRequired().HasMaxLength(50);
            builder.Property(e => e.Email).HasColumnName("Email").IsRequired().HasMaxLength(100);
            builder.Property(e => e.PasswordHash).HasColumnName("PasswordHash").IsRequired();
            builder.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired().HasDefaultValue(true);
            builder.Property(e => e.Role).HasColumnName("Role").IsRequired().HasDefaultValue(UserRole.Student);
            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            builder.HasMany(e => e.Quizzes)
                .WithOne(e => e.Creator)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.QuizResults)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 