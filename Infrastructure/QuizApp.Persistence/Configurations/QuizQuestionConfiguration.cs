using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> builder)
        {
            builder.HasKey(qq => qq.Id);

            // BaseEntity alanları
            builder.Property(qq => qq.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(qq => qq.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();

            builder.Property(qq => qq.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .IsRequired(false);

            builder.Property(qq => qq.DeletedDate)
                .HasColumnName("DeletedDate")
                .IsRequired(false);

            // Entity özel alanları
            builder.Property(qq => qq.QuizId)
                .HasColumnName("QuizId")
                .IsRequired();

            builder.Property(qq => qq.QuestionId)
                .HasColumnName("QuestionId")
                .IsRequired();

            builder.Property(qq => qq.OrderIndex)
                .HasColumnName("OrderIndex")
                .IsRequired();

            // İlişkiler
            builder.HasOne(qq => qq.Quiz)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(qq => qq.Question)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(qq => qq.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}