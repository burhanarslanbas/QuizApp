using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class QuizResultConfiguration : IEntityTypeConfiguration<QuizResult>
    {
        public void Configure(EntityTypeBuilder<QuizResult> builder)
        {
            builder.HasKey(qr => qr.Id);

            // BaseEntity alanları
            builder.Property(qr => qr.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(qr => qr.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();

            builder.Property(qr => qr.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .IsRequired(false);

            builder.Property(qr => qr.DeletedDate)
                .HasColumnName("DeletedDate")
                .IsRequired(false);

            // Entity özel alanları
            builder.Property(qr => qr.QuizId)
                .HasColumnName("QuizId")
                .IsRequired();

            builder.Property(qr => qr.UserId)
                .HasColumnName("UserId")
                .IsRequired();

            builder.Property(qr => qr.Score)
                .HasColumnName("Score")
                .IsRequired();

            builder.Property(qr => qr.EndTime)
                .HasColumnName("EndTime")
                .IsRequired(false);

            builder.Property(qr => qr.IsCompleted)
                .HasColumnName("IsCompleted")
                .IsRequired()
                .HasDefaultValue(false);

            // İlişkiler
            builder.HasOne(qr => qr.Quiz)
                .WithMany(q => q.QuizResults)
                .HasForeignKey(qr => qr.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(qr => qr.User)
                .WithMany()
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(qr => qr.UserAnswers)
                .WithOne(ua => ua.QuizResult)
                .HasForeignKey(ua => ua.QuizResultId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}