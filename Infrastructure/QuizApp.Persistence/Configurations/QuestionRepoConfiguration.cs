using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class QuestionRepoConfiguration : IEntityTypeConfiguration<QuestionRepo>
    {
        public void Configure(EntityTypeBuilder<QuestionRepo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasColumnName("Description").HasMaxLength(500);
            builder.Property(e => e.IsActive).HasColumnName("IsActive").IsRequired().HasDefaultValue(true);
            builder.Property(e => e.MaxQuestions).HasColumnName("MaxQuestions").IsRequired().HasDefaultValue(10);
            builder.Property(e => e.IsPublic).HasColumnName("IsPublic").IsRequired().HasDefaultValue(false);
            builder.Property(e => e.QuestionCount).HasColumnName("QuestionCount").IsRequired().HasDefaultValue(0);
            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");

            // Required foreign keys
            builder.Property(e => e.CreatorId).IsRequired();

            // Relationships
            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Questions)
                .WithOne(e => e.QuestionRepo)
                .HasForeignKey(e => e.QuestionRepoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 