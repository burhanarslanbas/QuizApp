using QuizApp.Domain.Entities.Common;

namespace QuizApp.Domain.Entities
{
    public class Option : BaseEntity
    {
        public Option()
        {
            UserAnswers = new HashSet<UserAnswer>();
        }
        public Guid QuestionId { get; set; } // Bağlı olduğu soru ID'si
        public string OptionText { get; set; } = default!; // Seçenek metni
        public bool IsCorrect { get; set; } = false; // Doğru cevap mı?
        public byte OrderIndex { get; set; } = default!;// Seçenek sırası

        public virtual Question Question { get; set; } = default!; // Bağlı olduğu soru
        public virtual ICollection<UserAnswer> UserAnswers { get; set; } // Bu seçeneği seçen cevaplar
    }
}