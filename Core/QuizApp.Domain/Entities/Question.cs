using QuizApp.Domain.Entities.Common;
using QuizApp.Domain.Enums;

namespace QuizApp.Domain.Entities
{
    public class Question : BaseEntity
    {
        public Question()
        {
            Options = new HashSet<Option>();
            UserAnswers = new HashSet<UserAnswer>();
            QuizQuestions = new HashSet<QuizQuestion>();
        }

        public Guid? QuestionRepoId { get; set; } // Bağlı olduğu soru reposu ID'si
        public QuestionType QuestionType { get; set; } = QuestionType.SingleChoice; // Soru tipi (varsayılan olarak tek seçimli)
        public string QuestionText { get; set; } = default!; // Soru metni
        public int Points { get; set; } = 1; // Soru puanı (varsayılan olarak 1)
        public int OrderIndex { get; set; } // Sıralama indeksi (soru listesinde sıralama için)
        public string? Explanation { get; set; } // Soru açıklaması (isteğe bağlı)
        public string? ImageUrl { get; set; } // Soruya ait resim URL'si (isteğe bağlı)
        public bool IsActive { get; set; } = true; // Soru aktif mi?

        // Navigation Properties
        public virtual QuestionRepo? QuestionRepo { get; set; }
        public virtual ICollection<Option> Options { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; }
    }
}