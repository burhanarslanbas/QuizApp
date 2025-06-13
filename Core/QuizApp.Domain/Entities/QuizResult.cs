using QuizApp.Domain.Entities.Common;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Domain.Entities
{
    public class QuizResult : BaseEntity
    {
        public QuizResult() => UserAnswers = new HashSet<UserAnswer>();
        public Guid QuizId { get; set; } // Quiz ID'si
        public Guid UserId { get; set; } // Öğrenci ID'si
        public int Score { get; set; } = 0; // Alınan puan
        public bool IsCompleted { get; set; } = false; // Tamamlanma durumu
        public DateTime? EndTime { get; set; } // Bitiş zamanı

        // Navigation Properties
        public virtual Quiz Quiz { get; set; } = default!; // Çözülen quiz
        public virtual AppUser User { get; set; } = default!; // Quiz'i çözen öğrenci
        public virtual ICollection<UserAnswer> UserAnswers { get; set; } // Verilen cevaplar
    }
}