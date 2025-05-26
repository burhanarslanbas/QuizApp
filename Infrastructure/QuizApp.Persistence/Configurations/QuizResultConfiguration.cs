using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class QuizResultConfiguration : IEntityTypeConfiguration<QuizResult>
    {
        public void Configure(EntityTypeBuilder<QuizResult> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Score).HasColumnName("Score").IsRequired().HasDefaultValue(0);
            builder.Property(e => e.IsCompleted).HasColumnName("IsCompleted").IsRequired().HasDefaultValue(false);
            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

            // Required foreign keys
            builder.Property(e => e.QuizId).IsRequired();
            builder.Property(e => e.StudentId).IsRequired();

            // Relationships
            builder.HasOne(e => e.Quiz)
                .WithMany(e => e.QuizResults)
                .HasForeignKey(e => e.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Student)
                .WithMany(e => e.QuizResults)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.StudentAnswers)
                .WithOne(e => e.QuizResult)
                .HasForeignKey(e => e.QuizResultId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 