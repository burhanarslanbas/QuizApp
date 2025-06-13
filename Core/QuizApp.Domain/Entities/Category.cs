using QuizApp.Domain.Entities.Common;

namespace QuizApp.Domain.Entities;

public class Category : BaseEntity
{
    public Category()
    {
        SubCategories = new HashSet<Category>();
        Quizzes = new HashSet<Quiz>();
    }
    public string Name { get; set; } = default!; // Kategori adı
    public string? Description { get; set; } // Kategori açıklaması
    public Guid? ParentCategoryId { get; set; } // Üst kategori ID'si
    public bool IsActive { get; set; } = true; // Aktiflik durumu

    // Navigation Properties
    public virtual Category? ParentCategory { get; set; } // Üst kategori
    public virtual ICollection<Category> SubCategories { get; set; } // Alt kategoriler
    public virtual ICollection<Quiz> Quizzes { get; set; } // Bağlı olduğu quizler  
}