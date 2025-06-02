using QuizApp.Domain.Entities.Common;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Domain.Entities;

public class Quiz : BaseEntity
{
    public Quiz()
    {
        Questions = new HashSet<Question>();
        QuizResults = new HashSet<QuizResult>();
    }
    public Guid CategoryId { get; set; } // Quiz'in ait olduğu kategori ID'si
    public Guid CreatorId { get; set; } // Quiz'i oluşturan kullanıcı ID'si
    public required string Title { get; set; } // Quiz başlığı
    public required string Description { get; set; } // Quiz açıklaması
    public required TimeSpan Duration { get; set; } // Quiz süresi
    public int PassingScore { get; set; } = 0; // Geçme notu
    public bool IsActive { get; set; } = true; // Aktiflik durumu
    public DateTime? StartDate { get; set; } // Başlangıç tarihi
    public DateTime? EndDate { get; set; } // Bitiş tarihi
    public int MaxAttempts { get; set; } = 1; // Maksimum deneme sayısı
    public bool ShowResults { get; set; } = true; // Sonuçları gösterme

    // Navigation Properties
    public virtual AppUser Creator { get; set; } = null!; // Quiz'i oluşturan kullanıcı
    public virtual Category Category { get; set; } = null!; // Quiz kategorisi
    public virtual ICollection<Question> Questions { get; set; } // Quiz soruları
    public virtual ICollection<QuizResult> QuizResults { get; set; } // Quiz sonuçları
}