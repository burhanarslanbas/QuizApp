using QuizApp.Domain.Entities.Common;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Domain.Entities;

public class Quiz : BaseEntity
{
    public Quiz()
    {
        QuizQuestions = new HashSet<QuizQuestion>();
        QuizResults = new HashSet<QuizResult>();
    }
    public Guid? CategoryId { get; set; } // Quiz'in ait olduğu kategori ID'si
    public Guid CreatorId { get; set; } = default!; // Quiz'i oluşturan kullanıcı ID'si
    public string Title { get; set; } = default!; // Quiz başlığı (Max 200 karakter)
    public string? Description { get; set; } // Quiz açıklaması (Max 1000 karakter)
    public TimeSpan TimeLimit { get; set; } = TimeSpan.FromMinutes(30); // Quiz süresi (varsayılan 30 dakika)
    public int? PassingScore { get; set; } // Geçme notu
    public int? MaxAttempts { get; set; } = 1; // Maksimum deneme sayısı
    public bool ShowResults { get; set; } = true; // Sonuçları gösterme
    public bool IsPublished { get; set; } = false; // Quiz yayınlanmış mı?
    public DateTime? PublishedDate { get; set; } // Quiz yayınlanma tarihi

    // Navigation Properties
    public virtual AppUser Creator { get; set; } = default!; // Quiz'i oluşturan kullanıcı
    public virtual Category? Category { get; set; } // Quiz kategorisi
    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } // Quiz soruları
    public virtual ICollection<QuizResult> QuizResults { get; set; } // Quiz sonuçları
}