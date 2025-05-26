using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.OptionText).HasColumnName("OptionText").IsRequired().HasMaxLength(500);
            builder.Property(e => e.IsCorrect).HasColumnName("IsCorrect").IsRequired().HasDefaultValue(false);
            builder.Property(e => e.OrderIndex).HasColumnName("OrderIndex").IsRequired();
            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            builder.HasOne(e => e.Question)
                .WithMany(e => e.Options)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.UserAnswers)
                .WithOne(e => e.Option)
                .HasForeignKey(e => e.OptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 