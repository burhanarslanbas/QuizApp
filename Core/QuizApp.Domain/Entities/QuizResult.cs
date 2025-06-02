using QuizApp.Domain.Entities.Common;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Domain.Entities
{
    public class QuizResult : BaseEntity
    {
        public QuizResult()
        {
            StudentAnswers = new HashSet<UserAnswer>();
        }
        public Guid QuizId { get; set; } // Quiz ID'si
        public Guid StudentId { get; set; } // Öğrenci ID'si
        public int Score { get; set; } = 0; // Alınan puan, varsayılan 0
        public bool IsCompleted { get; set; } = false; // Tamamlanma durumu, varsayılan false

        // Navigation Properties
        public virtual Quiz Quiz { get; set; } = default!; // Çözülen quiz
        public virtual AppUser Student { get; set; } = default!; // Quiz'i çözen öğrenci
        public virtual ICollection<UserAnswer> StudentAnswers { get; set; } // Verilen cevaplar
    }
}