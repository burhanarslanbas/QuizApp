using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.QuestionText).IsRequired();
            builder.Property(q => q.Points).IsRequired();
            builder.Property(q => q.OrderIndex).IsRequired();
            builder.Property(q => q.IsActive).IsRequired();
            builder.Property(q => q.QuestionTypeId).IsRequired();

            builder.HasOne(q => q.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(q => q.QuestionRepo)
                .WithMany(q => q.Questions)
                .HasForeignKey(q => q.QuestionRepoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.UserAnswers)
                .WithOne(ua => ua.Question)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 