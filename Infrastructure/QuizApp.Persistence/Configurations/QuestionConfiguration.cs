using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;
using QuizApp.Domain.Enums;

namespace QuizApp.Persistence.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);

            // BaseEntity alanları
            builder.Property(q => q.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(q => q.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();

            builder.Property(q => q.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .IsRequired(false);

            builder.Property(q => q.DeletedDate)
                .HasColumnName("DeletedDate")
                .IsRequired(false);

            // Entity özel alanları
            builder.Property(q => q.QuestionRepoId)
                .HasColumnName("QuestionRepoId")
                .IsRequired(false);

            builder.Property(q => q.QuestionType)
                .HasColumnName("QuestionType")
                .IsRequired()
                .HasDefaultValue(QuestionType.SingleChoice);

            builder.Property(q => q.QuestionText)
                .HasColumnName("QuestionText")
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(q => q.Points)
                .HasColumnName("Points")
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(q => q.Explanation)
                .HasColumnName("Explanation")
                .IsRequired(false)
                .HasMaxLength(2000);

            builder.Property(q => q.ImageUrl)
                .HasColumnName("ImageUrl")
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(q => q.IsActive)
                .HasColumnName("IsActive")
                .IsRequired()
                .HasDefaultValue(true);

            // İlişkiler
            builder.HasOne(q => q.QuestionRepo)
                .WithMany(qr => qr.Questions)
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

            builder.HasMany(q => q.QuizQuestions)
                .WithOne(qq => qq.Question)
                .HasForeignKey(qq => qq.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}