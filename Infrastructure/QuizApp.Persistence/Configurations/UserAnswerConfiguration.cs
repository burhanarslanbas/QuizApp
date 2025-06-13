using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.HasKey(ua => ua.Id);

            // BaseEntity alanları
            builder.Property(ua => ua.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(ua => ua.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();

            builder.Property(ua => ua.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .IsRequired(false);

            builder.Property(ua => ua.DeletedDate)
                .HasColumnName("DeletedDate")
                .IsRequired(false);

            // Entity özel alanları
            builder.Property(ua => ua.QuestionId)
                .HasColumnName("QuestionId")
                .IsRequired();

            builder.Property(ua => ua.QuizResultId)
                .HasColumnName("QuizResultId")
                .IsRequired(false);

            builder.Property(ua => ua.OptionId)
                .HasColumnName("OptionId")
                .IsRequired(false);

            builder.Property(ua => ua.TextAnswer)
                .HasColumnName("TextAnswer")
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.Property(ua => ua.IsCorrect)
                .HasColumnName("IsCorrect")
                .IsRequired()
                .HasDefaultValue(false);

            // İlişkiler
            builder.HasOne(ua => ua.QuizResult)
                .WithMany(qr => qr.UserAnswers)
                .HasForeignKey(ua => ua.QuizResultId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ua => ua.Question)
                .WithMany(q => q.UserAnswers)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ua => ua.Option)
                .WithMany(o => o.UserAnswers)
                .HasForeignKey(ua => ua.OptionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}