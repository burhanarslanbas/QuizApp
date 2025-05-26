using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.IsCorrect).HasColumnName("IsCorrect").IsRequired().HasDefaultValue(false);
            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

            // Required foreign keys
            builder.Property(e => e.QuizResultId).IsRequired();
            builder.Property(e => e.QuestionId).IsRequired();
            builder.Property(e => e.OptionId).IsRequired(false);

            // Relationships
            builder.HasOne(e => e.Question)
                .WithMany(e => e.UserAnswers)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.QuizResult)
                .WithMany(e => e.StudentAnswers)
                .HasForeignKey(e => e.QuizResultId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Option)
                .WithMany(e => e.UserAnswers)
                .HasForeignKey(e => e.OptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 