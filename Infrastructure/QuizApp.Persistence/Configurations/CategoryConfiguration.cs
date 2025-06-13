using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Entities;

namespace QuizApp.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            // BaseEntity alanları
            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(c => c.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();

            builder.Property(c => c.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .IsRequired(false);

            builder.Property(c => c.DeletedDate)
                .HasColumnName("DeletedDate")
                .IsRequired(false);

            // Entity özel alanları
            builder.Property(c => c.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasColumnName("Description")
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(c => c.IsActive)
                .HasColumnName("IsActive")
                .IsRequired()
                .HasDefaultValue(true);

            // Self-referencing relationship for parent-child categories
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category-Quiz relationship
            builder.HasMany(c => c.Quizzes)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}