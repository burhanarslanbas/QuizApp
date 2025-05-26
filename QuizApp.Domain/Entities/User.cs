using QuizApp.Domain.Entities.Common;
using QuizApp.Domain.Enums;

namespace QuizApp.Domain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Quizzes = new HashSet<Quiz>();
            QuizResults = new HashSet<QuizResult>();
        }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        public UserRole Role { get; set; } = UserRole.Student; // Varsayılan rol öğrenci

        // Navigation Properties
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<QuizResult> QuizResults { get; set; }
    }
}