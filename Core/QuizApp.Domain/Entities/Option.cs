using QuizApp.Domain.Entities.Common;

namespace QuizApp.Domain.Entities
{
    public class Option : BaseEntity
    {
        public Option() => UserAnswers = new HashSet<UserAnswer>();

        public Guid? QuestionId { get; set; } // Bağlı olduğu soru ID'si
        public string OptionText { get; set; } = default!; // Seçenek metni
        public byte OrderIndex { get; set; } = 0; // Sıralama indeksi (isteğe bağlı, varsayılan 0)
        public bool IsCorrect { get; set; } = false; // Seçeneğin doğru cevap olup olmadığı (varsayılan false)

        // Navigation Properties
        public virtual Question? Question { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}