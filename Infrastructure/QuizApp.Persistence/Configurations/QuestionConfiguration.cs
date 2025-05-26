using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.QuestionText).HasColumnName("QuestionText").IsRequired().HasMaxLength(1000);
            builder.Property(e => e.Points).HasColumnName("Points").IsRequired().HasDefaultValue(1);
            builder.Property(e => e.OrderIndex).HasColumnName("OrderIndex").IsRequired();
            builder.Property(e => e.QuestionType).HasColumnName("QuestionType").IsRequired();
            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            builder.HasOne(e => e.Quiz)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.QuestionRepo)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.QuestionRepoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Options)
                .WithOne(e => e.Question)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.UserAnswers)
                .WithOne(e => e.Question)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 