using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.HasKey(o => o.Id);

            // BaseEntity alanları
            builder.Property(o => o.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(o => o.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();

            builder.Property(o => o.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .IsRequired(false);

            builder.Property(o => o.DeletedDate)
                .HasColumnName("DeletedDate")
                .IsRequired(false);

            // Entity özel alanları
            builder.Property(o => o.QuestionId)
                .HasColumnName("QuestionId")
                .IsRequired(false);

            builder.Property(o => o.OptionText)
                .HasColumnName("OptionText")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(o => o.OrderIndex)
                .HasColumnName("OrderIndex")
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(o => o.IsCorrect)
                .HasColumnName("IsCorrect")
                .IsRequired()
                .HasDefaultValue(false);

            // İlişkiler
            builder.HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.UserAnswers)
                .WithOne(ua => ua.Option)
                .HasForeignKey(ua => ua.OptionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}