using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title).HasColumnName("Title").IsRequired().HasMaxLength(200);
            builder.Property(e => e.Description).HasColumnName("Description").IsRequired().HasMaxLength(1000);
            builder.Property(e => e.Duration).HasColumnName("Duration").IsRequired();
            builder.Property(e => e.PassingScore).HasColumnName("PassingScore").IsRequired().HasDefaultValue(0);
            builder.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired().HasDefaultValue(true);
            builder.Property(e => e.StartDate).HasColumnName("StartDate");
            builder.Property(e => e.EndDate).HasColumnName("EndDate");
            builder.Property(e => e.MaxAttempts).HasColumnName("MaxAttempts").IsRequired().HasDefaultValue(1);
            builder.Property(e => e.ShowResults).HasColumnName("ShowResults").IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

            // Required foreign keys
            builder.Property(e => e.CategoryId).IsRequired();
            builder.Property(e => e.CreatorId).IsRequired();

            // Relationships
            builder.HasOne(e => e.Creator)
                .WithMany(e => e.Quizzes)
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Category)
                .WithMany(e => e.Quizzes)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Questions)
                .WithOne(e => e.Quiz)
                .HasForeignKey(e => e.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.QuizResults)
                .WithOne(e => e.Quiz)
                .HasForeignKey(e => e.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 