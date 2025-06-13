using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FullName)
                .HasColumnName("FullName")
                .IsRequired()
                .HasMaxLength(100);

            // Relationships
            builder.HasMany(u => u.Quizzes)
                .WithOne(q => q.Creator)
                .HasForeignKey(q => q.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.QuizResults)
                .WithOne(qr => qr.User)
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}