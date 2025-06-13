using QuizApp.Domain.Entities.Common;

namespace QuizApp.Domain.Entities
{
    public class QuizQuestion : BaseEntity
    {
        public Guid QuizId { get; set; } = default!; // Quiz ID'si
        public Guid QuestionId { get; set; } = default!; // Soru ID'si
        public byte OrderIndex { get; set; } // Sıralama indeksi (quiz içindeki soru sırası)

        // Navigation Properties
        public virtual Quiz Quiz { get; set; } = default!;
        public virtual Question Question { get; set; } = default!;
    }
}