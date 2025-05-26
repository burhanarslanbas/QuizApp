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
        }
        public Guid QuizId { get; set; } // Bağlı olduğu quiz ID'si
        public Guid QuestionTypeId { get; set; } // Soru tipi ID'si
        public Guid? QuestionRepoId { get; set; } // Bağlı olduğu soru havuzu ID'si
        public QuestionType QuestionType { get; set; } = default!; // Soru tipi
        public string QuestionText { get; set; } = default!; // Soru metni
        public int Points { get; set; } = 1; // Soru puanı
        public int OrderIndex { get; set; } // Soru sırası

        // Navigation Properties
        public virtual Quiz? Quiz { get; set; } // Bağlı olduğu quiz
        public virtual QuestionRepo? QuestionRepo { get; set; } // Bağlı olduğu soru havuzu
        public virtual ICollection<Option> Options { get; set; } // Soru seçenekleri
        public virtual ICollection<UserAnswer> UserAnswers { get; set; } // Kullanıcı cevapları
    }
}