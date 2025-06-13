using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class QuestionRepoConfiguration : IEntityTypeConfiguration<QuestionRepo>
    {
        public void Configure(EntityTypeBuilder<QuestionRepo> builder)
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
            builder.Property(qr => qr.CreatorId)
                .HasColumnName("CreatorId")
                .IsRequired();

            builder.Property(qr => qr.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(qr => qr.Description)
                .HasColumnName("Description")
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(qr => qr.IsActive)
                .HasColumnName("IsActive")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(qr => qr.MaxQuestions)
                .HasColumnName("MaxQuestions")
                .IsRequired()
                .HasDefaultValue(50);

            builder.Property(qr => qr.IsPublic)
                .HasColumnName("IsPublic")
                .IsRequired()
                .HasDefaultValue(false);

            // İlişkiler
            builder.HasOne(qr => qr.Creator)
                .WithMany()
                .HasForeignKey(qr => qr.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(qr => qr.Questions)
                .WithOne(qr => qr.QuestionRepo)
                .HasForeignKey(qr => qr.QuestionRepoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}