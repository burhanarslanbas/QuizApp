
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
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
            builder.Property(q => q.CategoryId)
                .HasColumnName("CategoryId")
                .IsRequired(false);

            builder.Property(q => q.CreatorId)
                .HasColumnName("CreatorId")
                .IsRequired();

            builder.Property(q => q.Title)
                .HasColumnName("Title")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(q => q.Description)
                .HasColumnName("Description")
                .IsRequired(false)
                .HasMaxLength(1000);

            builder.Property(q => q.TimeLimit)
                .HasColumnName("TimeLimit")
                .IsRequired()
                .HasDefaultValue(TimeSpan.FromMinutes(30));

            builder.Property(q => q.PassingScore)
                .HasColumnName("PassingScore")
                .IsRequired(false);

            builder.Property(q => q.MaxAttempts)
                .HasColumnName("MaxAttempts")
                .IsRequired(false)
                .HasDefaultValue(1);

            builder.Property(q => q.ShowResults)
                .HasColumnName("ShowResults")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(q => q.IsPublished)
                .HasColumnName("IsPublished")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(q => q.PublishedDate)
                .HasColumnName("PublishedDate")
                .IsRequired(false);

            // İlişkiler
            builder.HasOne(q => q.Creator)
                .WithMany()
                .HasForeignKey(q => q.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.Category)
                .WithMany()
                .HasForeignKey(q => q.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.QuizQuestions)
                .WithOne(qq => qq.Quiz)
                .HasForeignKey(qq => qq.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}