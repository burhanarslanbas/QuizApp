using QuizApp.Domain.Entities.Common;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Domain.Entities
{
    public class QuestionRepo : BaseEntity
    {
        public QuestionRepo() => Questions = new HashSet<Question>();
        public Guid CreatorId { get; set; } // Oluşturan kullanıcı ID'si
        public string Name { get; set; } = default!; // Repo adı
        public string? Description { get; set; } // Repo açıklaması (Max 500 karakter)
        public bool IsActive { get; set; } = true; // Aktiflik durumu
        public int MaxQuestions { get; set; } = 50; // Maksimum soru sayısı
        public bool IsPublic { get; set; } = false; // Herkese açık mı?

        // Navigation Properties
        public virtual ICollection<Question> Questions { get; set; } // Soru listesi
        public virtual AppUser Creator { get; set; } = default!; // Oluşturan kullanıcı
    }
}