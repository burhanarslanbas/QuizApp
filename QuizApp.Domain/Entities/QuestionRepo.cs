using QuizApp.Domain.Entities.Common;

namespace QuizApp.Domain.Entities
{
    public class QuestionRepo : BaseEntity
    {
        public QuestionRepo()
        {
            Questions = new HashSet<Question>();
        }
        public Guid CreatorId { get; set; } // Oluşturan kullanıcı ID'si
        public string Name { get; set; } = default!; // Repo adı
        public string? Description { get; set; } // Repo açıklaması
        public bool IsActive { get; set; } = true; // Aktiflik durumu
        public int MaxQuestions { get; set; } = 10; // Maksimum soru sayısı
        public bool IsPublic { get; set; } = false; // Herkese açık mı?
        public int QuestionCount { get; set; } = 0; // Mevcut soru sayısı

        // Navigation Properties
        public virtual ICollection<Question> Questions { get; set; } // Soru listesi
        public virtual User Creator { get; set; } = default!; // Oluşturan kullanıcı
    }
}